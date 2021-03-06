using System;
using System.Collections.Generic;
namespace MascotasApi.Models
{
    public class Mascotas
    {
        public int Id { get; set; }
        public int idUsuario { get; set; }
        public int idRaza { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Foto { get; set; }
        public bool Castrada { get; set; }
        public bool Activo { get; set; }
        public Razas Raza { get; set; }
        public List<Reservas> Reservas { get; set; }
        public List<MascotasVacunas> MascotasVacuna { get; set; }

        public Usuarios Usuario { get; set; }

    }
}