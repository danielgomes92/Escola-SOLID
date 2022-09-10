using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Escola
{
    public class AlunoRepositorioJson
    {
        private string CaminhoJson()
        {
            return System.Configuration.ConfigurationManager.AppSettings["caminho_json"];
        }

        public List<Aluno> TodosJson()
        {
            var alunos = new List<Aluno>();
            if (File.Exists(this.CaminhoJson()))
            {
                var conteudo = File.ReadAllText(this.CaminhoJson());
                alunos = JsonConvert.DeserializeObject<List<Aluno>>(conteudo);
            }

            return alunos;
        }

        public void AdicionarJson(Aluno aluno)
        {
            var alunos = this.TodosJson();
            alunos.Add(aluno);
            File.WriteAllText(this.CaminhoJson(), JsonConvert.SerializeObject(alunos));
        }
    }
}
