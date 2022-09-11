using Escola.Entidades;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using Escola.Interfaces;
using System.Collections.Generic;

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
            return this.Todos().Count();
        }

        public List<Aluno> Todos()
        {
            var alunos = new List<Aluno>();
            if (File.Exists(CaminhoJson()))
            {
                var conteudo = File.ReadAllText(CaminhoJson());
                alunos = JsonConvert.DeserializeObject<List<Aluno>>(conteudo);
            }
            return alunos;
        }

        public void Salvar(Aluno aluno)
        {
            var alunos = this.Todos();
            alunos.Add(aluno);
            File.WriteAllText(CaminhoJson(), JsonConvert.SerializeObject(alunos));
        }
    }
}
