﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Agapea.App_Code.modelos;
namespace Agapea.App_Code.Controles_Personalizados
{
    public partial class MiniControlLibro : System.Web.UI.UserControl
    {
        #region "---propiedades del control"
        private string __titulo;
        private string __editorial;
        private string __autor;
        private string __precio;
        private string __ISBN;
        #endregion

        #region "controles getters y setters"
        public string TituloControl
        {
            get { return this.__titulo; }
            set { this.__titulo = value;
                this.linkButtonTitulo.Text = this.__titulo;
            }
        }
        public string EditorialControl
        {
            get { return this.__editorial; }
            set { this.__editorial = value;
                this.nombreEditorial.Text = this.__editorial;

            }
        }
        public string AutorControl
        {
            get { return this.__autor; }
            set { this.__autor = value;
                this.labelAutor.Text = this.__autor;
            }
        }
        public string PrecioControl
        {
            get { return this.__precio; }
            set { this.__precio = value;
                this.labelPrecio.Text = this.__precio;
            }
        }
        public string ISBNControl
        {
            get { return this.__ISBN; }
            set { this.__ISBN = value;
                this.labelISBN.Text = this.__ISBN;
            }
        }
        #endregion



        #region "...metodos clase..."

        //---------constructores-------
        public MiniControlLibro() { }

        public MiniControlLibro( Libro l ) {

            this.TituloControl = l.Titulo;
            this.EditorialControl = l.Editorial;
            this.AutorControl = l.Autor;
            this.PrecioControl = l.Precio;
            this.ISBNControl = l.ISBN10;
        }

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}