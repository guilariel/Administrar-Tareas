using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using static ConsoleApp15.Program;

namespace ConsoleApp15
{
    internal class Program
    {
        static void Main(string[] args)
        {
            AdministradorDeTareas administradorDeTareas = AdministradorDeTareas.GetInstance();
            AdministradorDeComandos adminComandos = new AdministradorDeComandos();

            Console.Write("Insertar título: ");
            string titulo = Console.ReadLine();
            Console.Write("Descripción de la tarea: ");
            string descripcion = Console.ReadLine();

            Tareas comandoAñadir = new  AñadirTareaNormal(titulo, descripcion, administradorDeTareas);
            adminComandos.EjecutarComando(comandoAñadir);

            Tareas comandoAñadir2 = new AñadirTareaNormal(titulo, descripcion, administradorDeTareas);
            adminComandos.EjecutarComando(comandoAñadir2);

            Console.WriteLine("\nTareas después de añadir:");
            administradorDeTareas.Mostrar();

            Console.WriteLine("\nDeshaciendo última acción...");
            adminComandos.DeshacerUltimoComando();

            // Mostrar tareas después de deshacer
            Console.WriteLine("\nTareas después de deshacer:");
            administradorDeTareas.Mostrar();

            Console.ReadLine();
        }


        public class AdministradorDeComandos
        {
            private Stack<Tareas> stack = new Stack<Tareas>();

            public void EjecutarComando(Tareas Tarea)
            {
                Tarea.Añadir();
                stack.Push(Tarea);
            }

            public void DeshacerUltimoComando()
            {
                if (stack.Count > 0)
                {
                    Tareas tarea = stack.Pop();
                    tarea.Desacer();
                }
                else
                {
                    Console.WriteLine("No hay acciones para deshacer.");
                }
            }
        }
        public class AdministradorDeTareas
        {
            private static AdministradorDeTareas administradorDeTareas;
            private Dictionary<string, string> tareas = new Dictionary<string, string>();

            private AdministradorDeTareas() { }

            public static AdministradorDeTareas GetInstance()
            {
                if (administradorDeTareas == null)
                {
                    administradorDeTareas = new AdministradorDeTareas();
                }

                return administradorDeTareas;
            }

            public void AñadirTarea(string titulo,string tarea)
            {
                tareas[titulo] = tarea;
            }

            public string Descripcion(string Titulo)
            {
                return tareas.ContainsKey(Titulo) ? tareas[Titulo] : null;
            }

            public void EliminarTarea(string titulo)
            {
                tareas.Remove(titulo);
            }

            public void Mostrar()
            {
                foreach(var tarea in tareas)
                {                  
                    Console.WriteLine($"titulo:{tarea.Key} tarea:{tarea.Value}");
                }
            }
        }
        public interface Tareas
        {
            void Añadir();
            void Desacer();
        }

        public class AñadirTareaNormal : Tareas
        {
            public string Titulo;
            public string Tarea;
            public AdministradorDeTareas administrador;

            public AñadirTareaNormal(string Titulo, string Tarea, AdministradorDeTareas administrador)
            {
                this.Titulo = Titulo;
                this.Tarea = Tarea;
                this.administrador = administrador;
            }

            public void Añadir()
            {
                AdministradorDeTareas.GetInstance().AñadirTarea(Titulo, Tarea);
            }

            public void Desacer()
            {
                AdministradorDeTareas.GetInstance().EliminarTarea(Titulo);
            }
        }

        /*public class EliminarTareaNormal : Tareas
        {
            public string Titulo;
            public string Tarea;
            public AdministradorDeTareas administrador;

            public EliminarTareaNormal(string Titulo, string Tarea, AdministradorDeTareas administrador)
            {
                this.Titulo = Titulo;
                this.Tarea = administrador.Descripcion(Titulo);
                this.administrador = administrador; 
            }

            public void Añadir()
            {
                AdministradorDeTareas.GetInstance().EliminarTarea(Titulo);
            }

            public void Desacer()
            {
                AdministradorDeTareas.GetInstance().AñadirTarea(Titulo, Tarea);
            }
        }

       /* public class CreadorDeTareas
        {
            public static Tareas NuevaTarea(string Tarea)
            {
                if (Tarea == "normal")
                {
                    return new AñadirTareaNormal();
                }

                return null;
            }
        }*/
    }
}

