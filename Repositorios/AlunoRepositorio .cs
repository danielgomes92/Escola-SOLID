using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Escola.Entidades;
using Escola.Interfaces;
using Newtonsoft.Json;

namespace Escola.Repositorios
{
    public class AlunoRepositorio
    {
        private IRepositorio repo;
        public AlunoRepositorio(IRepositorio repo)
        {
            this.repo = repo;
        }

        public int Quantidade()
        {
            return repo.Quantidade();
        }

        public List<Aluno> Todos()
        {
            return repo.Todos();
        }

        public void Salvar(Aluno aluno)
        {
            repo.Salvar(aluno);
        }
    }
}
