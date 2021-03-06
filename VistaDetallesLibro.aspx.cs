﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Agapea.App_Code.modelos;
using Agapea.App_Code.controladores;
using Agapea.ControlesUsuario;

namespace Agapea
{
    public partial class VistaDetallesLibro : System.Web.UI.Page
    {
        private Controlador_Acceso_Ficheros __controlInit;
        private ControladorTablas __controlTablas;
        private List<Libro> listaRecuperada;
        private string __rutaControl = "~/ControlesUsuario/MiniDetallesLibro.ascx";

        protected void Page_Load(object sender, EventArgs e)
        {
            #region "sesionUser"
            Label LabelUser = (Label)this.Master.FindControl("LabelUser");

            if (this.Session["Usuario"] != null)
            {
                LabelUser.Text = (string)Session["Usuario"];
            }
            else
            {
                LabelUser.Text = "Parece que el nombre no se almacena bien";
            }

            ///---llamada a procedimiento interno para ver variables post

            #endregion
            __controlTablas = new ControladorTablas(this.Page);
            __controlInit = new Controlador_Acceso_Ficheros();
            TreeView myTreeView = (TreeView)this.Master.FindControl("myTreeView");
            listaRecuperada = __controlInit.infoLibros("./Ficheros/libros.txt");

            string parametroISBN = this.Request.Params["isbn"];

            var resultado = from libro in listaRecuperada
                            where libro.ISBN10 == parametroISBN
                            select libro;

            Libro l = (Libro)resultado.Single();
            __controlTablas.rellenaControlDetallado(this.tablaDetallesLibro, l);

            if (this.IsPostBack)
            {
                foreach (string clave in this.Request.Params.AllKeys)
                {
                    string claveRequest = this.Request.Params[clave];

                    switch (clave)
                    {

                        case "__EVENTTARGET":

                            if (claveRequest.Contains(myTreeView.ID))
                            {
                                this.tablaDetallesLibro.Controls.Clear();
                                char[] separador = new char[] { '\\' };
                                string __categoriaPulsada = this.Request.Params["__EVENTARGUMENT"].ToString().Split(separador)[0].Substring(1);

                                if (__categoriaPulsada == "Todos")

                                    __controlTablas.rellenaTablaLibros(this.tablaDetallesLibro, listaRecuperada, false);
                                //rellenaTabla(librosFichero, false);

                                List<Libro> categoryList = __controlInit.findByCategory(listaRecuperada, __categoriaPulsada);

                                __controlTablas.rellenaTablaLibros(this.tablaDetallesLibro, categoryList, true);
                                //rellenaTabla(categoryList, true);
                            }
                            break;

                        default:
                            break;
                    }
                    if (clave.Contains("btncomprarDetalle"))
                    {
                        string isbnLibro = parametroISBN;
                        //ya tengo el ISBN porq me lo mando con la url.
                        if (Request.Cookies["cesta"] != null)
                        {
                            HttpCookie miCookie = Request.Cookies["cesta"];
                            miCookie.Values["libros"] += isbnLibro + ":";
                            Response.Cookies.Add(miCookie);
                        }

                        Response.Redirect("VistaCesta.aspx");
                    }


                }
            }
        }
    }
}