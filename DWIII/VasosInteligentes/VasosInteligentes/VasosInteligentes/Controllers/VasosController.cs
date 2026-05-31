using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VasosInteligentes.Data;
using VasosInteligentes.Models;

namespace VasosInteligentes.Controllers
{
    public class VasosController : Controller
    {
        private readonly ContextMongoDb _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public VasosController(ContextMongoDb context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Roles = "Administrador")]
        // GET: Vasos
        public async Task<IActionResult> Index()
        {
            var pipeline = new BsonDocument[]
            {
                // Cria campos temporários que serão utilizados na conversão de object para string
                new BsonDocument("$addFields", new BsonDocument
                {
                    { "PlantaIdObj", new BsonDocument("$toObjectId", "$PlantaId") }
                }),

                // Faz o "join" usando o campo convertido
                new BsonDocument("$lookup", new BsonDocument
                {
                    { "from", "Planta" },
                    { "localField", "PlantaIdObj" },
                    { "foreignField", "_id" },
                    { "as", "PlantaRelacionada" }
                }),

                // Remove os camposs extras para não quebrar o C#
                new BsonDocument("$project", new BsonDocument
                {
                    { "PlantaIdObj", 0 }
                })
            };
            var result = await _context.Vaso.Aggregate<Vaso>(pipeline).ToListAsync();
            return View(result);
        }

        [Authorize(Roles = "Usuario")]
        // GET: Vasos
        public async Task<IActionResult> MeusVasos()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            var usuarioId = user.Id;

            var pipeline = new BsonDocument[]
            {
                new BsonDocument("$match", new BsonDocument("UsuarioId", usuarioId)),
                // Cria campos temporários que serão utilizados na conversão de object para string
                new BsonDocument("$addFields", new BsonDocument
                {
                    { "PlantaIdObj", new BsonDocument("$toObjectId", "$PlantaId") }
                }),

                // Faz o "join" usando o campo convertido
                new BsonDocument("$lookup", new BsonDocument
                {
                    { "from", "Planta" },
                    { "localField", "PlantaIdObj" },
                    { "foreignField", "_id" },
                    { "as", "PlantaRelacionada" }
                }),

                // Remove os camposs extras para não quebrar o C#
                new BsonDocument("$project", new BsonDocument
                {
                    { "PlantaIdObj", 0 }
                })
            };
            var result = await _context.Vaso.Aggregate<Vaso>(pipeline).ToListAsync();
            return View(result);
        }

        // =====================================================================
        // NOVO MÉTODO: Dashboard
        // =====================================================================
        // [Authorize(Roles = "Usuario")] → garante que apenas usuários comuns
        // autenticados possam acessar esta action. O administrador NÃO tem acesso.
        //
        // O método busca todos os vasos do usuário logado e, para cada vaso,
        // calcula a média de luminosidade separada em três períodos do dia:
        //   • Manhã  → leituras entre 06:00 e 11:59
        //   • Tarde  → leituras entre 12:00 e 17:59
        //   • Noite  → leituras entre 18:00 e 00:00 (até meia-noite)
        //
        // Os dados são enviados para a View via ViewBag em formato JSON,
        // prontos para serem consumidos pelo ApexCharts.
        // =====================================================================
        [Authorize(Roles = "Usuario")]
        public async Task<IActionResult> Dashboard()
        {
            // 1. Obtém o usuário logado
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Accounts");
            }

            // 2. Busca todos os vasos que pertencem ao usuário logado
            var vasos = await _context.Vaso
                .Find(v => v.UsuarioId == user.Id)
                .ToListAsync();

            // 3. Listas que vão guardar as médias de cada período para cada vaso.
            //    Cada lista terá um valor por vaso, na mesma ordem da lista "vasos".
            var mediasManha = new List<double>();  // médias do período da Manhã
            var mediasTarde = new List<double>();  // médias do período da Tarde
            var mediasNoite = new List<double>();  // médias do período da Noite
            var nomesVasos = new List<string>();  // nomes dos vasos (rótulos do eixo X)

            // 4. Para cada vaso, busca as leituras e calcula as médias por período
            foreach (var vaso in vasos)
            {
                // Adiciona o nome do vaso à lista de rótulos
                nomesVasos.Add(vaso.Nome ?? "Sem nome");

                // Busca TODAS as leituras do vaso (sem filtro de data,
                // para considerar o histórico completo no cálculo das médias)
                var leituras = await _context.LeituraSensor
                    .Find(l => l.VasoId == vaso.Id)
                    .ToListAsync();

                // --- Cálculo da média de Manhã (06:00 – 11:59) ---
                // Filtra as leituras cujo horário (hora local) esteja entre 6 e 11
                var leiturasManha = leituras
                    .Where(l => l.DataLeitura.ToLocalTime().Hour >= 6 &&
                                l.DataLeitura.ToLocalTime().Hour < 12)
                    .ToList();

                // Se existir ao menos uma leitura no período, calcula a média;
                // caso contrário, usa 0 para não quebrar o gráfico
                mediasManha.Add(leiturasManha.Any()
                    ? Math.Round(leiturasManha.Average(l => l.Luminosidade), 2)
                    : 0);

                // --- Cálculo da média de Tarde (12:00 – 17:59) ---
                var leiturasTarde = leituras
                    .Where(l => l.DataLeitura.ToLocalTime().Hour >= 12 &&
                                l.DataLeitura.ToLocalTime().Hour < 18)
                    .ToList();

                mediasTarde.Add(leiturasTarde.Any()
                    ? Math.Round(leiturasTarde.Average(l => l.Luminosidade), 2)
                    : 0);

                // --- Cálculo da média de Noite (18:00 – 00:00) ---
                // Inclui da hora 18 até a hora 23 (ou seja, até as 23:59)
                var leiturasNoite = leituras
                    .Where(l => l.DataLeitura.ToLocalTime().Hour >= 18)
                    .ToList();

                mediasNoite.Add(leiturasNoite.Any()
                    ? Math.Round(leiturasNoite.Average(l => l.Luminosidade), 2)
                    : 0);
            }

            // 5. Serializa os dados para JSON e envia para a View via ViewBag.
            //    O System.Text.Json é usado aqui para gerar strings JSON simples
            //    que serão embutidas diretamente no JavaScript do ApexCharts.
            ViewBag.NomesVasos = System.Text.Json.JsonSerializer.Serialize(nomesVasos);
            ViewBag.MediasManha = System.Text.Json.JsonSerializer.Serialize(mediasManha);
            ViewBag.MediasTarde = System.Text.Json.JsonSerializer.Serialize(mediasTarde);
            ViewBag.MediasNoite = System.Text.Json.JsonSerializer.Serialize(mediasNoite);

            return View();
        }
        // =====================================================================
        // FIM DO NOVO MÉTODO: Dashboard
        // =====================================================================

        // GET: Vasos/Details/5
        [Authorize(Roles = "Usuario")]
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var vaso = await _context.Vaso.Find(v => v.Id == id).FirstOrDefaultAsync();
            if (vaso == null)
            {
                return NotFound();
            }

            // Busca os dados da planta que está no vaso
            if (!string.IsNullOrEmpty(vaso.PlantaId))
            {
                var planta = await _context.Planta.Find(p => p.Id == vaso.PlantaId).FirstOrDefaultAsync();
                if (planta != null)
                {
                    vaso.PlantaRelacionada = new List<Planta> { planta };
                }
            }

            // Busca os dados das leituras
            // Limita em 24h 
            var limiteData = DateTime.UtcNow.AddHours(-24);
            var historicoLeituras = await _context.LeituraSensor.Find(l => l.VasoId == id && l.DataLeitura >= limiteData)
                .SortByDescending(l => l.DataLeitura)
                .ToListAsync();
            ViewBag.HistoricoLeituras = historicoLeituras;
            return View(vaso);
        }

        // GET: Vasos/Create
        public async Task<IActionResult> Create()
        {
            var plantas = await _context.Planta.Find(_ => true).ToListAsync();
            ViewBag.PlantaId = new SelectList(plantas, "Id", "Nome");
            return View();
        }

        // POST: Vasos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Usuario")]
        public async Task<IActionResult> Create([Bind("Id,Nome,PlantaId,Localizacao")] Vaso vaso)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            var usuarioId = user.Id;
            ModelState.Remove("UsuarioId");

            if (ModelState.IsValid)
            {
                vaso.UsuarioId = usuarioId;
                await _context.Vaso.InsertOneAsync(vaso);
                return RedirectToAction(nameof(MeusVasos));
            }
            return View(vaso);
        }

        // GET: Vasos/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vaso = await _context.Vaso.Find(m => m.Id == id).FirstOrDefaultAsync();
            if (vaso == null)
            {
                return NotFound();
            }
            var plantas = await _context.Planta.Find(_ => true).ToListAsync();
            ViewBag.PlantaId = new SelectList(plantas, "Id", "Nome", vaso.PlantaId);
            return View(vaso);
        }

        // POST: Vasos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Usuario")]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Nome,PlantaId,Localizacao,UsuarioId")] Vaso vaso) // ✅ adicionou UsuarioId no Bind
        {
            if (id != vaso.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.Vaso.ReplaceOneAsync(p => p.Id == vaso.Id, vaso);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await VasoExists(vaso.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(MeusVasos));
            }
            return View(vaso);
        }

        // GET: Vasos/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pipeline = new BsonDocument[]
            {
                new BsonDocument("$match", new BsonDocument("_id", new BsonObjectId(new ObjectId(id)))),
                // Cria campos temporários que serão utilizados na conversão de object para string
                new BsonDocument("$addFields", new BsonDocument
                {
                    { "PlantaIdObj", new BsonDocument("$toObjectId", "$PlantaId") }
                }),

                // Faz o "join" usando o campo convertido
                new BsonDocument("$lookup", new BsonDocument
                {
                    { "from", "Planta" },
                    { "localField", "PlantaIdObj" },
                    { "foreignField", "_id" },
                    { "as", "PlantaRelacionada" }
                }),

                // Remove os camposs extras para não quebrar o C#
                new BsonDocument("$project", new BsonDocument
                {
                    { "PlantaIdObj", 0 }
                })
            };
            var result = await _context.Vaso.Aggregate<Vaso>(pipeline).FirstOrDefaultAsync();
            return View(result);
        }

        // POST: Vasos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Usuario")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await _context.Vaso.DeleteOneAsync(m => m.Id == id);
            return RedirectToAction(nameof(MeusVasos));
        }

        private async Task<bool> VasoExists(string id)
        {
            return await _context.Vaso.Find(e => e.Id == id).AnyAsync();
        }
    }
}
