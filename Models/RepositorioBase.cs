using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tp_Inmobiliaria_Ledesma_Lillo.Models
{
    public abstract class RepositorioBase
    {
        protected readonly String connectionString;
        protected readonly IConfiguration configuration;

        protected RepositorioBase(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.connectionString = configuration["ConnectionStrings:DefaultConnection"];
        }
    }
}