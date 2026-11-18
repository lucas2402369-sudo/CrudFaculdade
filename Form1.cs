using CrudTesteDev.DAL;
using CrudTesteDev.Entities;
using System;
using System.Windows.Forms;
using System.IO;
using System.Text;
namespace CrudTesteDev
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

         
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                PessoaDAL dal = new PessoaDAL();

                dgvPessoas.DataSource = dal.ListarTodos();

                dgvPessoas.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Erro ao carregar os cadastros:\n" + ex.Message,
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

     

        private void cbSexo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void labelPesquisar_Click(object sender, EventArgs e)
        {

        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            tbCodigo.Clear();
            tbNome.Clear();

            cbSexo.SelectedIndex = -1;

            rbCliente.Checked = false;
            rbFuncionario.Checked = false;

            tbPesquisar.Clear();

            dgvPessoas.ClearSelection();

            tbNome.Focus();

        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {

            string nome = tbNome.Text.Trim();

            if (string.IsNullOrWhiteSpace(nome))
            {
                MessageBox.Show("Informe o nome da pessoa.");
                tbNome.Focus();
                return;
            }

            if (cbSexo.SelectedIndex == -1)
            {
                MessageBox.Show("Selecione o sexo.");
                cbSexo.Focus();
                return;
            }

            if (!rbCliente.Checked && !rbFuncionario.Checked)
            {
                MessageBox.Show("Selecione Cliente ou Funcionário.");
                return;
            }

            Pessoa pessoa = new Pessoa();

            pessoa.Nome = nome;
            pessoa.Sexo = cbSexo.SelectedItem.ToString();
            pessoa.TipoPessoa = rbCliente.Checked
                ? "Cliente"
                : "Funcionario";

            try
            {
                PessoaDAL dal = new PessoaDAL();

                int codigoGerado = dal.Salvar(pessoa);

                dgvPessoas.DataSource = null;
                dgvPessoas.DataSource = dal.ListarTodos();
                dgvPessoas.ClearSelection();

                tbCodigo.Text = codigoGerado.ToString();

                MessageBox.Show(
                    "Cadastro realizado com sucesso!\nCódigo: " +
                    codigoGerado,
                    "Sucesso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Erro ao salvar cadastro:\n" + ex.Message,
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }

        }

        private void dgvPessoas_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex < 0)
                return;

            Pessoa pessoa = dgvPessoas.Rows[e.RowIndex]
                .DataBoundItem as Pessoa;

            if (pessoa == null)
                return;

            tbCodigo.Text = pessoa.Codigo.ToString();
            tbNome.Text = pessoa.Nome;

            cbSexo.SelectedItem = pessoa.Sexo;

            rbCliente.Checked = pessoa.TipoPessoa == "Cliente";
            rbFuncionario.Checked = pessoa.TipoPessoa == "Funcionario";

        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {

            int codigo;

            if (!int.TryParse(tbCodigo.Text, out codigo))
            {
                MessageBox.Show("Selecione uma pessoa na tabela.");
                return;
            }

            string nome = tbNome.Text.Trim();

            if (string.IsNullOrWhiteSpace(nome))
            {
                MessageBox.Show("Informe o nome da pessoa.");
                tbNome.Focus();
                return;
            }

            if (cbSexo.SelectedIndex == -1)
            {
                MessageBox.Show("Selecione o sexo.");
                return;
            }

            if (!rbCliente.Checked && !rbFuncionario.Checked)
            {
                MessageBox.Show("Selecione Cliente ou Funcionário.");
                return;
            }

            Pessoa pessoa = new Pessoa();

            pessoa.Codigo = codigo;
            pessoa.Nome = nome;
            pessoa.Sexo = cbSexo.SelectedItem.ToString();
            pessoa.TipoPessoa = rbCliente.Checked
                ? "Cliente"
                : "Funcionario";

            try
            {
                PessoaDAL dal = new PessoaDAL();

                bool atualizado = dal.Atualizar(pessoa);

                if (atualizado)
                {
                    dgvPessoas.DataSource = null;
                    dgvPessoas.DataSource = dal.ListarTodos();
                    dgvPessoas.ClearSelection();

                    MessageBox.Show(
                        "Cadastro atualizado com sucesso!",
                        "Sucesso",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                else
                {
                    MessageBox.Show("Cadastro não encontrado.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Erro ao atualizar cadastro:\n" + ex.Message,
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }

        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {

            int codigo;

            if (!int.TryParse(tbCodigo.Text, out codigo))
            {
                MessageBox.Show("Selecione uma pessoa para excluir.");
                return;
            }

            DialogResult confirmacao = MessageBox.Show(
                "Deseja realmente excluir este cadastro?",
                "Confirmar Exclusão",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirmacao != DialogResult.Yes)
            {
                return;
            }

            try
            {
                PessoaDAL dal = new PessoaDAL();

                bool excluido = dal.Deletar(codigo);

                if (excluido)
                {
                    dgvPessoas.DataSource = null;
                    dgvPessoas.DataSource = dal.ListarTodos();
                    dgvPessoas.ClearSelection();

                    tbCodigo.Clear();
                    tbNome.Clear();
                    cbSexo.SelectedIndex = -1;

                    rbCliente.Checked = false;
                    rbFuncionario.Checked = false;

                    MessageBox.Show(
                        "Cadastro excluído com sucesso!",
                        "Sucesso",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );

                    tbNome.Focus();
                }
                else
                {
                    MessageBox.Show("Cadastro não encontrado.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Erro ao excluir cadastro:\n" + ex.Message,
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }

        }

        private void btnPesquisar_Click(object sender, EventArgs e)
        {

            try
            {
                var dal = new PessoaDAL();
                string nome = tbPesquisar.Text.Trim();

                if (string.IsNullOrWhiteSpace(nome))
                {
                    dgvPessoas.DataSource = dal.ListarTodos();
                }
                else
                {
                    dgvPessoas.DataSource = dal.PesquisarPorNome(nome);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao pesquisar: " + ex.Message);
            }

        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            if (dgvPessoas.Rows.Count == 0)
            {
                MessageBox.Show("Não existem cadastros para exportar.");
                return;
            }

            using (SaveFileDialog salvar = new SaveFileDialog())
            {
                salvar.Title = "Exportar Cadastros";
                salvar.Filter = "Arquivo CSV (*.csv)|*.csv";
                salvar.FileName = "Cadastros_Pessoas.csv";
                salvar.DefaultExt = "csv";
                salvar.OverwritePrompt = true;

                if (salvar.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                try
                {
                    using (StreamWriter arquivo = new StreamWriter(
                        salvar.FileName, false, new UTF8Encoding(true)))
                    {
                        arquivo.WriteLine("Codigo;Nome;Sexo;TipoPessoa");

                        foreach (DataGridViewRow linha in dgvPessoas.Rows)
                        {
                            Pessoa pessoa = linha.DataBoundItem as Pessoa;

                            if (pessoa == null)
                            {
                                continue;
                            }

                            string nome = pessoa.Nome ?? "";
                            string sexo = pessoa.Sexo ?? "";
                            string tipo = pessoa.TipoPessoa ?? "";

                            // Protege contra fórmulas no Excel
                            if (nome.TrimStart().StartsWith("=") ||
                                nome.TrimStart().StartsWith("+") ||
                                nome.TrimStart().StartsWith("-") ||
                                nome.TrimStart().StartsWith("@"))
                            {
                                nome = "'" + nome;
                            }

                            // Trata aspas presentes nos dados
                            nome = nome.Replace("\"", "\"\"");
                            sexo = sexo.Replace("\"", "\"\"");
                            tipo = tipo.Replace("\"", "\"\"");

                            arquivo.WriteLine(
                                pessoa.Codigo + ";\"" +
                                nome + "\";\"" +
                                sexo + "\";\"" +
                                tipo + "\"");
                        }
                    }

                    MessageBox.Show(
                        "Cadastros exportados com sucesso!",
                        "Exportação concluída",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "Erro ao exportar os cadastros:\n" + ex.Message,
                        "Erro",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
        }
    }
}

