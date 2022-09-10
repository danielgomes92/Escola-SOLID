using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Escola
{
    public class Aluno
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Matricula { get; set; }
        public List<double> Notas { get; set; }

        public double Media()
        {
            double somaNotas = 0;
            foreach (var nota in this.Notas)
                somaNotas += nota;

            return somaNotas / this.Notas.Count;
        }

        public string Situacao()
        {
            return (this.Media() > 6 ? "Aprovado" : "Reprovado");
        }

        public string NotasFormadata()
        {
            return string.Join(",", this.Notas);
        }

        private static List<Aluno> alunos = new List<Aluno>();
        public static List<Aluno> TodosJson()                       //JSON
        {
            if (File.Exists(Aluno.CaminhoJson()))
            {
                var conteudo = File.ReadAllText(Aluno.CaminhoJson());
                Aluno.alunos = JsonConvert.DeserializeObject<List<Aluno>>(conteudo);
            }

            return Aluno.alunos;
        }

        //SQL
        public static List<Aluno> TodosSql()                       //SQL
        {
            Aluno.alunos = new List<Aluno>();
            using (var cnn = new SqlConnection(Aluno.stringConexaoSql()))
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

                            Aluno.alunos.Add(aluno);
                                                                                    
                        }
                    }

                    //Outro reader
                    foreach (var aluno in Aluno.alunos)
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

            return Aluno.alunos;
        }
        private static string stringConexaoSql()
        {
            return System.Configuration.ConfigurationManager.AppSettings["conexao_sql"];
        }
        //SQL

        private static string CaminhoJson()
        {
            return System.Configuration.ConfigurationManager.AppSettings["caminho_json"];
        }

        public static void AdicionarJson(Aluno aluno)       //JSON
        {
            Aluno.alunos = Aluno.TodosJson();                        //JSON
            Aluno.alunos.Add(aluno);
            string caminho = @"E:\PersistenciadeDados\alunos.json";
            File.WriteAllText(Aluno.CaminhoJson(), JsonConvert.SerializeObject(Aluno.alunos));
        }

        public static void AdicionarSql(Aluno aluno)
        {
            using (var cnn = new SqlConnection(Aluno.stringConexaoSql()))
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
