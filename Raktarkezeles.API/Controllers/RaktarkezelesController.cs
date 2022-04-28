using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Raktarkezeles.API.Data;
using Raktarkezeles.API.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Raktarkezeles.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RaktarkezelesController : ControllerBase
    {
        private readonly RaktarkezelesContext _context;

        public RaktarkezelesController(RaktarkezelesContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("/alkatreszek")]
        public async Task<ActionResult<List<int>>> GetAlkatreszIds(string kereses = "")
        {
            if (kereses == "")
            {
                var result = await _context.Alkatreszek.Select(x => x.Id).ToListAsync();
                return Ok(result);
            }
            else
            {
                var result = await _context.Alkatreszek
               .Where(x => x.Nev.Contains(kereses) || x.Gyarto.TeljesNev.Contains(kereses) || x.Kategoria.Nev.Contains(kereses) || x.Tipus.Contains(kereses) || x.Cikkszam.Contains(kereses) || x.Leiras.Contains(kereses))
               .Select(x => x.Id)
               .ToListAsync();
                return Ok(result);
            }
        }

        [HttpGet]
        [Route("/alkatreszek/{id}")]
        public async Task<ActionResult<Alkatresz>> GetAlkatresz(int id)
        {
            var result = await _context.Alkatreszek
                .Where(x => x.Id == id)
                .Include(x => x.Gyarto)
                .Include(x => x.Kategoria)
                .Include(x => x.MennyisegiEgyseg)
                .Include(x => x.AlkatreszElofordulasok).ThenInclude(x => x.RaktarozasiHely)
                .AsSplitQuery().FirstOrDefaultAsync();
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("/alkatreszek")]
        public async Task<ActionResult<AlkatreszDTO>> PostAlkatresz([FromBody] AlkatreszDTO alkatresz)
        {
            if (!await AlkatreszValid(alkatresz))
            {
                return BadRequest();
            }
            var ujAlkatresz = new Alkatresz(alkatresz);
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.Alkatreszek.AddAsync(ujAlkatresz);
                    await _context.SaveChangesAsync();
                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                }
            }
            return StatusCode(201, new AlkatreszDTO(ujAlkatresz));
        }

        [HttpPut]
        [Route("/alkatreszek/{id}")]
        public async Task<ActionResult<AlkatreszDTO>> PutAlkatresz([FromRoute] int id, [FromBody] AlkatreszDTO alkatresz)
        {
            var result = await _context.Alkatreszek.FindAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            if (!await AlkatreszValid(alkatresz))
            {
                return BadRequest();
            }
            result.GyartoId = alkatresz.GyartoId;
            result.KategoriaId = alkatresz.KategoriaId;
            result.MennyisegiEgysegId = alkatresz.MennyisegiEgysegId;
            result.Nev = alkatresz.Nev;
            result.Tipus = alkatresz.Tipus;
            result.Cikkszam = alkatresz.Cikkszam;
            result.Leiras = alkatresz.Leiras;
            _context.Alkatreszek.Update(result);
            await _context.SaveChangesAsync();
            return Ok(new AlkatreszDTO(result));
        }

        [HttpDelete]
        [Route("/alkatreszek/{id}")]
        public async Task<ActionResult> DeleteAlkatresz(int id)
        {
            var result = await _context.Alkatreszek.FindAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            var elofordulasok = _context.AlkatreszElofordulasok.Where(x => x.AlkatreszId == id);
            _context.AlkatreszElofordulasok.RemoveRange(elofordulasok);
            _context.Alkatreszek.Remove(result);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        [Route("/alkatreszek/{id}/foto")]
        public async Task<ActionResult> GetAlkatreszFoto(int id)
        {
            var result = await _context.Alkatreszek.FindAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            var image = new MemoryStream(result.Foto);
            return File(image, "image/jpeg");
        }

        [HttpPost]
        [Route("/alkatreszek/{id}/foto")]
        public async Task<ActionResult> PostAlkatreszFoto(int id)
        {
            var result = await _context.Alkatreszek.FindAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            foreach (var file in Request.Form.Files)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    result.Foto = memoryStream.ToArray();
                }
            }
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        [Route("/elofordulasok/{id}")]
        public async Task<ActionResult<AlkatreszElofordulas>> GetElofordulas(int id)
        {
            var result = await _context.AlkatreszElofordulasok.Include(x => x.RaktarozasiHely).FirstOrDefaultAsync(x => x.Id == id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("/elofordulasok")]
        public async Task<ActionResult<AlkatreszElofordulasDTO>> PostElofordulas([FromBody] AlkatreszElofordulasDTO elofordulas)
        {
            if (!await AlkatreszElofordulasValid(elofordulas))
            {
                return BadRequest();
            }
            var ujElofordulas = new AlkatreszElofordulas(elofordulas);
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.AlkatreszElofordulasok.AddAsync(ujElofordulas);
                    await _context.SaveChangesAsync();
                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                }
            }
            return StatusCode(201, new AlkatreszElofordulasDTO(ujElofordulas));
        }

        [HttpPatch]
        [Route("/elofordulasok/{id}")]
        public async Task<ActionResult> PatchElofordulas(int id, [FromBody] JsonPatchDocument<AlkatreszElofordulas> patchDoc)
        {
            if (patchDoc != null)
            {
                var result = await _context.AlkatreszElofordulasok.FindAsync(id);
                if (result == null)
                {
                    return NotFound();
                }
                foreach (var operation in patchDoc.Operations)
                {
                    if (operation.op != "add" || operation.path != "/mennyiseg")
                    {
                        return StatusCode(405);
                    }
                }
                patchDoc.ApplyTo(result);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete]
        [Route("/elofordulasok/{id}")]
        public async Task<ActionResult> DeleteElofordulas(int id)
        {
            var result = await _context.AlkatreszElofordulasok.FindAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            _context.AlkatreszElofordulasok.Remove(result);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        [Route("/raktarak")]
        public async Task<ActionResult<List<RaktarozasiHely>>> GetRaktarozasiHelyek()
        {
            var result = await _context.RaktarozasiHelyek.ToListAsync();
            return Ok(result);
        }

        [HttpGet]
        [Route("/gyartok")]
        public async Task<ActionResult<List<Gyarto>>> GetGyartok()
        {
            var result = await _context.Gyartok.ToListAsync();
            return Ok(result);
        }

        [HttpGet]
        [Route("/kategoriak")]
        public async Task<ActionResult<List<Kategoria>>> GetKategoriak()
        {
            var result = await _context.Kategoriak.ToListAsync();
            return Ok(result);
        }

        [HttpGet]
        [Route("/egysegek")]
        public async Task<ActionResult<List<MennyisegiEgyseg>>> GetMennyisegiEgysegek()
        {
            var result = await _context.MennyisegiEgysegek.ToListAsync();
            return Ok(result);
        }

        private async Task<bool> AlkatreszValid(AlkatreszDTO alkatresz)
        {
            bool gyartoHelyes = await _context.Gyartok.AnyAsync(x => x.Id == alkatresz.GyartoId);
            bool kategoriaHelyes = await _context.Kategoriak.AnyAsync(x => x.Id == alkatresz.KategoriaId);
            bool egysegHelyes = await _context.MennyisegiEgysegek.AnyAsync(x => x.Id == alkatresz.MennyisegiEgysegId);
            bool alkatreszHelyes = !string.IsNullOrWhiteSpace(alkatresz.Nev) && !string.IsNullOrWhiteSpace(alkatresz.Tipus) && !string.IsNullOrWhiteSpace(alkatresz.Cikkszam);
            return gyartoHelyes && kategoriaHelyes && egysegHelyes && alkatreszHelyes;
        }
        private async Task<bool> AlkatreszElofordulasValid(AlkatreszElofordulasDTO alkatreszElofordulas)
        {
            bool alkatreszHelyes = await _context.Alkatreszek.AnyAsync(x => x.Id == alkatreszElofordulas.AlkatreszId);
            bool raktarHelyes = await _context.RaktarozasiHelyek.AnyAsync(x => x.Id == alkatreszElofordulas.RaktarozasiHelyId);
            bool elofordulasHelyes = alkatreszElofordulas.Mennyiseg >= 0;
            return alkatreszHelyes && raktarHelyes && elofordulasHelyes;
        }
    }
}
