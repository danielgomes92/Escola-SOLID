using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Escola.Entidades;
using Escola.Interfaces;

namespace Escola.Repositorios
{
    public class AlunoRepositorio
    {
        private IRepositorio repo;
        public AlunoRepositorio(IRepositorio repo) // injeção de dependência para onde vou salvar
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
