﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Agapea.App_Code.controladores;
using Agapea.App_Code.modelos;
using Agapea.ControlesUsuario;



namespace Agapea
{
    public partial class MyInicio : System.Web.UI.Page
    {
        //hay que cambiar el ControladorFichero
        //meter todo el Codigo de ControladorInicio.
        private Controlador_Acceso_Ficheros __controlFichero;
        private ControladorTablas __controlTabla;
        private string __categoriaPulsada;
        private List<Libro> librosFichero;
        private const string __rutaControlLibro = "~/ControlesUsuario/MiniLibro.ascx";
        private const string __rutaControlDetalles = "~/ControlesUsuario/MiniDetallesLibro.ascx";
        private const string __rutaControlCesta = "~/ControlesUsuario/MiniCesta.ascx";
        
        protected void Page_Load(object sender, EventArgs e)
        {
            #region "controles de la Master"
            ImageButton ButtonCompra = (ImageButton)this.Master.FindControl("ButtonCompra");
            Label LabelUser = (Label)this.Master.FindControl("LabelUser");
            TreeView myTreeView = (TreeView)this.Master.FindControl("myTreeView");
            ContentPlaceHolder holder = (ContentPlaceHolder)this.Master.FindControl("placeHolderControl");
            #endregion
            MiniCesta __controlCesta = (MiniCesta)Page.LoadControl(__rutaControlCesta);
            holder.Controls.Add(__controlCesta);

            __controlFichero = new Controlador_Acceso_Ficheros();
            __controlTabla = new ControladorTablas(this.Page);

            librosFichero = __controlFichero.infoLibros("./Ficheros/libros.txt");

            #region "sesionUser"
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

            mostrar();

            #region "isPostback"
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
                                char[] separador = new char[] { '\\' };
                                __categoriaPulsada = this.Request.Params["__EVENTARGUMENT"].ToString().Split(separador)[0].Substring(1);
                                LabelUser.Text = "has seleccionado el nodo treeview: " + __categoriaPulsada;
                                //metodo que devuelve libros con categorías....

                                if (__categoriaPulsada == "Todos")

                                    __controlTabla.rellenaTablaLibros(this.tablaLibros, librosFichero, false);

                                List<Libro> categoryList = __controlFichero.findByCategory(librosFichero, __categoriaPulsada);

                                __controlTabla.rellenaTablaLibros(this.tablaLibros, categoryList, true);
                            }

                            else if (claveRequest.Contains("linkButtonTitulo"))
                            {
                                string isbnLibro = ((string)claveRequest).Substring(((string)claveRequest).Length - 10, 10);
                                Response.Redirect("VistaDetallesLibro.aspx?isbn=" + isbnLibro);
                            }


                            break;

                        default:
                            break;

                    }
                    if (clave.Contains("botonCompra") )
                    {
                        string isbnLibro = ((string)clave).Substring(((string)clave).Length - 10, 10);

                        if ( Request.Cookies["cesta"] != null )
                        {
                            HttpCookie miCookie = Request.Cookies["cesta"];
                            miCookie.Values["libros"] += isbnLibro + ":";
                            Response.Cookies.Add(miCookie);
                        }
                     
                        __controlCesta.addItem();
                        __controlTabla.rellenaTablaLibros(this.tablaLibros, librosFichero, false);
                    }
                    else if (clave.Contains("botonVerCesta"))
                    {
                       
                        Response.Redirect("VistaCesta.aspx");
                    }
                }
            }
            #endregion
            else
            {
                Response.Cookies["cesta"]["owner"] = (string)this.Session["Usuario"];
                Response.Cookies["cesta"]["libros"] = "";
                Response.Cookies["cesta"].Expires = DateTime.Now.AddDays(1);
                __controlTabla.rellenaTablaLibros(this.tablaLibros, librosFichero, false);

            }

        }

        private void mostrar()
        {
            string mensaje = "";
            foreach (string clave in this.Request.Params.Keys)
            {
                mensaje += "clave:_" + clave + " -------valor:_" +
                    this.Request.Params[clave].ToString() + "\n";
            }
            this.TextBox1.Text = mensaje;
        }
    }
}
