using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace modelos
{
    
    public class ApplicationDbContext: DbContext
    {
            protected override void OnModelCreating(ModelBuilder modelBuilder)
        {   
            modelBuilder.Entity<usuario>(entity => {
            entity.HasIndex(e => e.correo).IsUnique();

            modelBuilder.Entity<movimiento>().HasOne(p => p.Usuario).WithMany(x=> x.movimientos);
        });
    }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

        public DbSet<usuario> Usuario { get; set; }
        public DbSet<usuario> usuarios {get; set;}
        public DbSet<recurso> recursos {get; set;}
        public DbSet<lugar> lugares {get; set;}
        public DbSet<movimiento> movimientos {get; set;}
        public DbSet<geolug> geolugares {get; set;}
        public DbSet<punto> puntos {get; set;}
        public DbSet<ent_sal> EntradasSalidas {get; set;}
        public DbSet<posicion> posiciones {get; set;}
    }
    [Table("Usuario")]
    public class usuario
    {
       
        [Key]
        
        public int? Usuarioid {get; set;}
        public string nombre {get; set;}
        [EmailAddress,Required(ErrorMessage ="Se Requiere el correo.")]
        public string correo {get; set;}
        [MinLength(8,ErrorMessage="El password requiere minimo 8 caracteres"),Required (ErrorMessage ="El password es requerido")]
        public string password {get; set;}
        public string role {get;set;} = "user";
        public bool active {get;set;} = true;
        
        public List<movimiento> movimientos {get; set;}
        

    }
    [Table("Recurso")]
    public class recurso{
       
        [Key]
        public int RecursoId{get; set;}
        [Required(ErrorMessage = "Se requiere el nombre.")]
        public string nombre {get;set;}
        [Required(ErrorMessage = "Se requiere el tipo.")]
        public string tipo{get;set;}
        public bool active{get;set;} = true;  
        public List<movimiento> movimientos {get; set;}

    }
    [Table("Lugar")]
    public class lugar{
        [Key]
        public int LugarId{ get; set;}
        [Required(ErrorMessage = "Se requiere el nombre.")]
        public string nombre {get;set;}
        [Required(ErrorMessage = "Se requiere el domicilio.")]
        public string domicilio {get;set;}
        
        [Column(TypeName = "decimal(9, 6)")]
        [Required (ErrorMessage = "Se requiere la latitud.")]
        public decimal latitud {get;set;}
        
        [Column(TypeName = "decimal(9, 6)")]
        [Required(ErrorMessage = "Se requiere la longitud.")]
        public decimal longitud {get; set;}
        public bool active {get;set;} = true;       

    }
    [Table("Usu_Rec")]
    public class movimiento
    {
        [Key]
        public int MovId{ get; set;}
        
        public int UsuarioId {get; set;}
        [ForeignKey("UsuarioId")]
        public usuario Usuario {get; set;}
       
        public int RecursoId {get; set;}
        [ForeignKey("RecursoId")]
        public recurso Recurso{get;set;}
        [Required(ErrorMessage = "Se requiere La fecha de inicio.")]
        public DateTime FInicio {get; set;} = DateTime.Now;
        public DateTime FFin {get; set;} = DateTime.Now.AddMonths(1);
        public bool active {get;set;} = true;   

         

    }

    public class geolug{
         [Key]
        public int GLId{ get; set;}
       
        [Required]
        [ForeignKey("LugarId")]
        public int LugId {get; set;}    
        public bool active {get;set;} = true;    


    }

    [Index(nameof(LugarId))]
    public class punto{
        [Key]
        public int PuntoId{get; set;}
        public int LugarId {get; set;}
         [Required(ErrorMessage = "Se requiere latitud.")]
         [Column(TypeName = "decimal(9, 6)")]
        public decimal latitud {get;set;}
        [Required(ErrorMessage = "Se requiere longitud.")]
        [Column(TypeName = "decimal(9, 6)")]
        public decimal longitud {get; set;}
        public bool active {get;set;} = true;       
       
    }

    public class posicion {
        
        [Key]
        public int PosId { get; set;}
        [ForeignKey("RecursoId")]
        public int RecursoId{get;set;}

        [Required(ErrorMessage = "Se requiere latitud.")]
        [Column(TypeName = "decimal(9, 6)")]
        public decimal latitud {get;set;}
        [Required(ErrorMessage = "Se requiere longitud.")]
        [Column(TypeName = "decimal(9, 6)")]
        public decimal longitud {get; set;}
        public DateTime FStamp {get; set;} = DateTime.Now;

    
    }

    public class ent_sal{
        [Key]
        public int EntSalId{get; set;}
        [Required]
        [ForeignKey("RecursoId")]
         public int RecursoId { get; set; }
         [Required]
         [ForeignKey("GLId")]
        public int GLId { get; set; }
        [Required]
        public DateTime Entrada {get; set;}
        public DateTime Salida {get; set;}
        public bool active {get;set;} = true;    


    }

    public class npunto{
         
        public decimal lat {get;set;}
      
       
        public decimal lng {get; set;}
    }

   
}