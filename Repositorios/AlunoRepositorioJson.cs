using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using Escola.Entidades;
using Escola.Interfaces;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Escola.Repositorios
{
    public class AlunoRepositorioJson : IRepositorio
    {
        private string CaminhoJson()
        {
            return System.Configuration.ConfigurationManager.AppSettings["caminho_json"];
        }

        public int Quantidade()
        {
            return this.Todos().Count;
        }

        public List<Aluno> Todos()
        {
            var alunos = new List<Aluno>();
            if (File.Exists(this.CaminhoJson()))
            {
                var conteudo = File.ReadAllText(this.CaminhoJson());
                alunos = JsonConvert.DeserializeObject<List<Aluno>>(conteudo);
            }

            return alunos;
        }

        public void Salvar(Aluno aluno)
        {
            var alunos = this.Todos();
            alunos.Add(aluno);
            File.WriteAllText(this.CaminhoJson(), JsonConvert.SerializeObject(alunos));
        }
    }
}
