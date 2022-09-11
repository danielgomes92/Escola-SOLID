using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Escola.Entidades;

namespace Escola.Interfaces
{
    public interface IRepositorio
    {
        int Quantidade();
        List<Aluno> Todos();
        void Salvar(Aluno aluno);
    }
}
