using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Escola.Repositorios
{
    public class AlunoRepositorio
    {
        public void Salvar(Aluno aluno)
        {
            if (aluno.OndeSalvar == Enum.OndeSalvar.Arquivo)
            {
                new AlunoRepositorioJson().Salvar(aluno);
            }
            else if (aluno.OndeSalvar == Enum.OndeSalvar.Sql)
            {
                new AlunoRepositorioSql().Salvar(aluno);
            }
            else
            {
                throw new Exception("Tipo inexistente!");
            }
        } 
    }
}
