using System;
namespace MascotasApi.Models
{
    public class Calendario
    {
        public int Id { get; set; }
        public int MascotasTipoId { get; set; }
        public int VacunaId { get; set; }
        public string Enfermedades { get; set; }
        public string Edad { get; set; }
        public bool Optativa { get; set; }
        public string Periodicidad { get; set; }
        public bool Activo { get; set; }
        public MascotasTipo MascotasTipo { get; set; }
        public Vacunas Vacuna { get; set; }

    }
}
