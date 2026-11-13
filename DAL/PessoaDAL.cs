using CrudTesteDev.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace CrudTesteDev.DAL
{
    public class PessoaDAL
    {
        private readonly string connectionString;

        public PessoaDAL()
        {
            var configuracao =
                ConfigurationManager.ConnectionStrings["ConexaoDB"];

            if (configuracao == null)
            {
                throw new Exception(
                    "Conexão ConexaoDB não encontrada no App.config.");
            }

            connectionString = configuracao.ConnectionString;
        }

        public List<Pessoa> ListarTodos()
        {
            List<Pessoa> pessoas = new List<Pessoa>();

            string sql = @"SELECT Codigo, Nome, Sexo, TipoPessoa
                           FROM dbo.Pessoa
                           ORDER BY Nome";

            using (SqlConnection conexao =
                new SqlConnection(connectionString))
            {
                using (SqlCommand comando =
                    new SqlCommand(sql, conexao))
                {
                    conexao.Open();

                    using (SqlDataReader leitor =
                        comando.ExecuteReader())
                    {
                        while (leitor.Read())
                        {
                            Pessoa pessoa = new Pessoa();

                            pessoa.Codigo =
                                Convert.ToInt32(leitor["Codigo"]);

                            pessoa.Nome =
                                Convert.ToString(leitor["Nome"]);

                            pessoa.Sexo =
                                Convert.ToString(leitor["Sexo"]);

                            pessoa.TipoPessoa =
                                Convert.ToString(leitor["TipoPessoa"]);

                            pessoas.Add(pessoa);
                        }
                    }
                }
            }

            return pessoas;
        }

        public int Salvar(Pessoa pessoa)
        {
            string sql = @"INSERT INTO dbo.Pessoa
                   (Nome, Sexo, TipoPessoa)
                   OUTPUT INSERTED.Codigo
                   VALUES (@Nome, @Sexo, @TipoPessoa)";

            using (SqlConnection conexao =
                new SqlConnection(connectionString))
            {
                using (SqlCommand comando =
                    new SqlCommand(sql, conexao))
                {
                    comando.Parameters.Add(
                        "@Nome", SqlDbType.NVarChar, 100
                    ).Value = pessoa.Nome;

                    comando.Parameters.Add(
                        "@Sexo", SqlDbType.NVarChar, 20
                    ).Value = pessoa.Sexo;

                    comando.Parameters.Add(
                        "@TipoPessoa", SqlDbType.NVarChar, 20
                    ).Value = pessoa.TipoPessoa;

                    conexao.Open();

                    int codigoGerado =
                        Convert.ToInt32(comando.ExecuteScalar());

                    return codigoGerado;
                }
            }
        }

        public bool Atualizar(Pessoa pessoa)
        {
            string sql = @"UPDATE dbo.Pessoa
                   SET Nome = @Nome,
                       Sexo = @Sexo,
                       TipoPessoa = @TipoPessoa
                   WHERE Codigo = @Codigo";

            using (SqlConnection conexao =
                new SqlConnection(connectionString))
            {
                using (SqlCommand comando =
                    new SqlCommand(sql, conexao))
                {
                    comando.Parameters.Add(
                        "@Codigo", SqlDbType.Int
                    ).Value = pessoa.Codigo;

                    comando.Parameters.Add(
                        "@Nome", SqlDbType.NVarChar, 100
                    ).Value = pessoa.Nome;

                    comando.Parameters.Add(
                        "@Sexo", SqlDbType.NVarChar, 20
                    ).Value = pessoa.Sexo;

                    comando.Parameters.Add(
                        "@TipoPessoa", SqlDbType.NVarChar, 20
                    ).Value = pessoa.TipoPessoa;

                    conexao.Open();

                    int linhasAfetadas = comando.ExecuteNonQuery();

                    return linhasAfetadas > 0;
                }
            }
        }


        public bool Deletar(int codigo)
        {
            string sql = @"DELETE FROM dbo.Pessoa
                   WHERE Codigo = @Codigo";

            using (SqlConnection conexao =
                new SqlConnection(connectionString))
            {
                using (SqlCommand comando =
                    new SqlCommand(sql, conexao))
                {
                    comando.Parameters.Add(
                        "@Codigo", SqlDbType.Int
                    ).Value = codigo;

                    conexao.Open();

                    int linhasAfetadas = comando.ExecuteNonQuery();

                    return linhasAfetadas > 0;
                }
            }
        }

        public List<Pessoa> PesquisarPorNome(string nome)
        {
            List<Pessoa> pessoas = new List<Pessoa>();

            string sql = @"SELECT Codigo, Nome, Sexo, TipoPessoa
                   FROM dbo.Pessoa
                   WHERE Nome LIKE @Nome
                   ORDER BY Nome";

            using (SqlConnection conexao =
                new SqlConnection(connectionString))
            {
                using (SqlCommand comando =
                    new SqlCommand(sql, conexao))
                {
                    comando.Parameters.Add(
                        "@Nome", SqlDbType.NVarChar, 102
                    ).Value = "%" + nome + "%";

                    conexao.Open();

                    using (SqlDataReader leitor =
                        comando.ExecuteReader())
                    {
                        while (leitor.Read())
                        {
                            Pessoa pessoa = new Pessoa();

                            pessoa.Codigo =
                                Convert.ToInt32(leitor["Codigo"]);

                            pessoa.Nome =
                                Convert.ToString(leitor["Nome"]);

                            pessoa.Sexo =
                                Convert.ToString(leitor["Sexo"]);

                            pessoa.TipoPessoa =
                                Convert.ToString(leitor["TipoPessoa"]);

                            pessoas.Add(pessoa);
                        }
                    }
                }
            }

            return pessoas;
        }

        public bool Existe(int codigo)
        {
            string sql = @"SELECT COUNT(*)
                   FROM dbo.Pessoa
                   WHERE Codigo = @Codigo";

            using (SqlConnection conexao =
                new SqlConnection(connectionString))
            {
                using (SqlCommand comando =
                    new SqlCommand(sql, conexao))
                {
                    comando.Parameters.Add(
                        "@Codigo", SqlDbType.Int
                    ).Value = codigo;

                    conexao.Open();

                    int quantidade =
                        Convert.ToInt32(comando.ExecuteScalar());

                    return quantidade > 0;
                }
            }
        }

    }
}
