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
        public List<Aluno> TodosJson()                       //JSON
        {
            var alunos = new List<Aluno>();
            if (File.Exists(this.CaminhoJson()))
            {
                var conteudo = File.ReadAllText(this.CaminhoJson());
                alunos = JsonConvert.DeserializeObject<List<Aluno>>(conteudo);
            }

            return alunos;
        }

        //SQL
        public List<Aluno> TodosSql()                       //SQL
        {
            var alunos = new List<Aluno>();
            using (var cnn = new SqlConnection(this.stringConexaoSql()))
            {
                cnn.Open();
                using (var cmd = new SqlCommand("select * from alunos", cnn)) // cmd = Command
                {
                    using (SqlDataReader dr = cmd.ExecuteReader()) // dr = Data Reader
                    {
                        while (dr.Read()) // enquanto estiver lendo vai ser transformado na lista de aluno
                        {
                            var aluno = new Aluno();
                            aluno.Id = Convert.ToInt32(dr["id"]);
                            aluno.Nome = dr["nome"].ToString();
                            aluno.Matricula = dr["matricula"].ToString();

                            alunos.Add(aluno);

                        }
                    }

                    //Outro reader
                    foreach (var aluno in alunos)
                    {
                        using (var cmdNotas = new SqlCommand("select * from notas where aluno_id=" + aluno.Id, cnn)) // cmd = Command
                        {
                            using (SqlDataReader drNotas = cmdNotas.ExecuteReader()) // dr = Data Reader
                            {
                                aluno.Notas = new List<double>();
                                while (drNotas.Read()) // enquanto estiver lendo vai ser transformado na lista de aluno
                                {
                                    aluno.Notas.Add(Convert.ToDouble(drNotas["nota"]));
                                }
                            }
                        }
                    }
                }
                cnn.Close();
            }

            return alunos;
        }
        private string stringConexaoSql()
        {
            return System.Configuration.ConfigurationManager.AppSettings["conexao_sql"];
        }
        //SQL

        private string CaminhoJson()
        {
            return System.Configuration.ConfigurationManager.AppSettings["caminho_json"];
        }

        public void AdicionarJson(Aluno aluno)       //JSON
        {
            var alunos = this.TodosJson();                        //JSON
            alunos.Add(aluno);
            File.WriteAllText(this.CaminhoJson(), JsonConvert.SerializeObject(alunos));
        }

        public void AdicionarSql(Aluno aluno)
        {
            using (var cnn = new SqlConnection(this.stringConexaoSql()))
            {
                cnn.Open();
                var cmd = new SqlCommand("insert into alunos(nome, matricula) values (@nome, @matricula);select @@identity", cnn);
                cmd.Parameters.AddWithValue("@nome", aluno.Nome);
                cmd.Parameters.AddWithValue("@matricula", aluno.Matricula);
                int aluno_id = Convert.ToInt32(cmd.ExecuteScalar());

                foreach (var nota in aluno.Notas)
                {
                    var cmdNota = new SqlCommand("insert into notas(aluno_id, nota) values (@aluno_id, @nota)", cnn);
                    cmdNota.Parameters.AddWithValue("@aluno_id", aluno_id);
                    cmdNota.Parameters.AddWithValue("@nota", nota);
                    cmdNota.ExecuteNonQuery();
                }
                cnn.Close();
            }
        }
    }
}
