using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Icao;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace uc9_MySQL1
{
    public partial class Form1 : Form
    {
        private MySqlConnection Conecao;
        private string data_source = "datasource=localhost;username=root;password=root;database=db_agenda";
        private int? id_Contato = null;
        public Form1()
        {
            InitializeComponent();

            lsvLista.View = View.Details;
            lsvLista.LabelEdit = true;
            lsvLista.AllowColumnReorder = true;
            lsvLista.GridLines = true;
            

            lsvLista.Columns.Add("ID", 30, HorizontalAlignment.Left);
            lsvLista.Columns.Add("Nome", 150, HorizontalAlignment.Left);
            lsvLista.Columns.Add("Telefone", 150, HorizontalAlignment.Left);
            lsvLista.Columns.Add("Email", 150, HorizontalAlignment.Left);

        }

        private void btnGravar_Click(object sender, EventArgs e)
        {
            try
            {
                
                
                //criar conexao MySql
                Conecao = new MySqlConnection(data_source);

                MySqlCommand cmd = new MySqlCommand();

                cmd.Connection = Conecao;

                Conecao.Open();

                cmd.Parameters.AddWithValue("@ID", id_Contato);
                cmd.Parameters.AddWithValue("@NOME", txtNome.Text);
                cmd.Parameters.AddWithValue("@EMAIL", txtEmail.Text);
                cmd.Parameters.AddWithValue("@TELEFONE", txtTelefone.Text);

                if (id_Contato == null)
                {
                    cmd.CommandText = "INSERT INTO contato (nome, email, telefone)" + "VALUES " + "(@NOME, @EMAIL, @TELEFONE)";
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Id foi!");
                }
                else
                {
                    cmd.CommandText = "UPDATE contato SET nome=@NOME, email=@EMAIL, telefone=@TELEFONE WHERE id=@ID";
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Sucesso!",
                        "Contato atualizado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                //execultar comando insert
                //cmd.Connection = Conecao;
                //cmd.CommandText = "INSERT INTO contato(nome, email, telefone)" +
                //                  "VALUES " +
                //                  "(@NOME, @EMAIL, @TELEFONE)";
                
                //cmd.Prepare();

                //cmd.ExecuteNonQuery();

                //MessageBox.Show("Foi!");

            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show("Error " + ex.Number + " has occurred: " + ex.Message,
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Conecao.Close();
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                string q = "'%" + txtBusca.Text + "%'";
                
                MessageBox.Show(q);

                //criar conexao MySQL
                Conecao = new MySqlConnection(data_source);

                string sql = "SELECT * " +
                             "FROM contato " +
                             "WHERE nome LIKE " + q + "OR email LIKE" + q;

                Conecao.Open();

                MessageBox.Show(sql);

                //executar comando
                MySqlCommand comando = new MySqlCommand(sql, Conecao);

                MySqlDataReader reader = comando.ExecuteReader();
                        
                lsvLista.Items.Clear();

                while (reader.Read())
                {
                    //percorre toda a tabela para achar o oque esta procurando
                    string[] row =
                    {
                        reader.GetValue(0).ToString(),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetString(3)
                    };

                    var linha_listview = new ListViewItem(row);


                    lsvLista.Items.Add(linha_listview);
                }

            MessageBox.Show("Deu certo de novo!!!");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
               
            }
            finally
            {
                Conecao.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string q;

                q = "'%" + txtDelete + "%'";

                MessageBox.Show(q);

                //criar conexao MySQL
                Conecao = new MySqlConnection(data_source);

                string sql = "DELETE " +
                             "FROM contato " +
                             "WHERE nome LIKE " + q + "OR email LIKE" + q;

                Conecao.Open();

                MessageBox.Show(sql);

                //executar comando
                MySqlCommand comando = new MySqlCommand(sql, Conecao);

                MySqlDataReader reader = comando.ExecuteReader();

                lsvLista.Items.Clear();

                while (reader.Read())
                {
                    //percorre toda a tabela para achar o oque esta procurando
                    string[] row =
                    {
                        reader.GetValue(0).ToString(),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetString(3)
                    };

                    var linha_listview = new ListViewItem(row);


                    lsvLista.Items.Add(linha_listview);
                }

                MessageBox.Show("Deu certo de novo!!!");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Conecao.Close();
            }
        }

        private void lsvLista_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            ListView.SelectedListViewItemCollection items_sel = lsvLista.SelectedItems;

            foreach (ListViewItem item in items_sel)
            {
                //id_Contato = Convert.ToInt32(item.SubItems[0].Text;

                txtNome.Text = item.SubItems[1].Text;
                txtEmail.Text = item.SubItems[2].Text;
                txtTelefone.Text = item.SubItems[3].Text;
            }
        }
    }
}
