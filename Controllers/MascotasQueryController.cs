using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MascotasApi.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using MascotasApi.Helpers;


namespace MascotasApi.Controllers
{

    public class MascotasQueryController : ControllerBase
    {
        private readonly MascotasContext _context;

        private readonly Permissions _permissions;

        public MascotasQueryController(MascotasContext context)
        {
            _context = context;
            _permissions = new Permissions();
        }

        // GET: api/Mascotas

        [EnableQuery(MaxExpansionDepth = 3)]
        [AllowAnonymous]
        public IQueryable<Mascotas> Get()
        {

            IQueryable<Mascotas> mascotas;

            if (_permissions.isAdmin(this.User))
            {
                mascotas = _context.Mascotas.AsQueryable();
            }
            else
            {
                int idUsuario = _permissions.getUserId(this.User);
                mascotas = _context.Mascotas.Where(m => m.idUsuario == idUsuario).AsQueryable();
            }
            return mascotas;
        }
    }
}
