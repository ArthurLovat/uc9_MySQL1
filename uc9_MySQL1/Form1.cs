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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace uc9_MySQL1
{
    public partial class Form1 : Form
    {
        private MySqlConnection Connection;
        private string data_source = "datasource=localhost;username=root;password=root;database=db_agenda";
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

        }

        private void btnGravar_Click(object sender, EventArgs e)
        {
            try
            {
                
                
                //criar conexao MySql
                Connection = new MySqlConnection(data_source);

                //execultar comando insert
                string sql = "INSERT INTO contato (nome, email, telefone) " +
                "VALUES('" + txtNome.Text + "','" + txtEmail.Text + "', '" + txtTelefone.Text + "')";

                //exibir o comando inserir dados
                MessageBox.Show(sql);
                MySqlCommand comando = new MySqlCommand(sql, Connection);

                Connection.Open();

                comando.ExecuteReader();

                MessageBox.Show("Foi!");

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: ", ex.Message);
               
            }
            finally
            {
                Connection.Close();
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                string q = "'%" + txtBusca.Text + "%'";
                
                MessageBox.Show(q);

                //criar conexao MySQL
                Connection = new MySqlConnection(data_source);

                string sql = "SELECT * " +
                             "FROM contato " +
                             "WHERE nome LIKE " + q + "OR email LIKE" + q;

                Connection.Open();

                MessageBox.Show(sql);

                //executar comando
                MySqlCommand comando = new MySqlCommand(sql, Connection);

                MySqlDataReader reader = comando.ExecuteReader();
                        
                lsvLista.Items.Clear();

                while (reader.Read())
                {

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
                Connection.Close();
            }
        }
    }
}
