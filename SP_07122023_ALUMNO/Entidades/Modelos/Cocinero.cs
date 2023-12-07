using Entidades.DataBase;
using Entidades.Exceptions;
using Entidades.Files;
using Entidades.Interfaces;
using System.Runtime.CompilerServices;

namespace Entidades.Modelos
{
    public delegate void DelegadoDemoraAtencion(double demora);
    public delegate void DelegadoPedidoEnCurso(IComestible pedidoEnPreparacion);

    public class Cocinero<T> where T : IComestible, new()
    {
        private int cantPedidosFinalizados;
        private string nombre;
        private double demoraPreparacionTotal;
        private CancellationTokenSource cancellation;
        private T pedidoEnPreparacion;
        private Queue<T> pedidos;
        private Mozo<T> mozo;
        private Task tarea;
        public event DelegadoDemoraAtencion OnDemora;
        public event DelegadoPedidoEnCurso OnPedido;


        public Cocinero(string nombre)
        {
            this.nombre = nombre;
            this.mozo = new Mozo<T>();
            this.pedidos = new Queue<T>();
            this.mozo.OnPedido += this.TomarNuevoPedido;
        }

        //No hacer nada
        public bool HabilitarCocina
        {
            get
            {
                return this.tarea is not null && (this.tarea.Status == TaskStatus.Running ||
                    this.tarea.Status == TaskStatus.WaitingToRun ||
                    this.tarea.Status == TaskStatus.WaitingForActivation);
            }
            set
            {
                if (value && !this.HabilitarCocina)
                {
                    this.cancellation = new CancellationTokenSource();
                    this.mozo.EmpezarATrabajar = true;
                    this.EmpezarACocinar();
                    
                }
                else
                {
                    
                    this.cancellation.Cancel();
                    this.mozo.EmpezarATrabajar = false;
                }
            }
        }

        public Queue<T> Pedidos
        {
            get { return this.pedidos;}
        }

        //no hacer nada
        public double TiempoMedioDePreparacion { get => this.cantPedidosFinalizados == 0 ? 0 : this.demoraPreparacionTotal / this.cantPedidosFinalizados; }
        public string Nombre { get => nombre; }
        public int CantPedidosFinalizados { get => cantPedidosFinalizados; }

        private void EmpezarACocinar()
        {
            
            if (!cancellation.IsCancellationRequested && this.OnPedido is not null)
            {
                if (this.pedidos.Count > 0)
                {
                    this.pedidoEnPreparacion = this.pedidos.Dequeue();
                    OnPedido.Invoke(this.pedidoEnPreparacion);
                    this.EsperarProximoIngreso();
                    this.cantPedidosFinalizados += 1;
                    //DataBaseManager.GuardarTicket<T>(Nombre, this.pedidoEnPreparacion);
                }

                
            }
            
        }

        /*private void NotificarNuevoIngreso()
        {
            if (this.OnPedido is not null)
            {
                
                this.pedidoEnPreparacion = new T();
                this.pedidoEnPreparacion.IniciarPreparacion();
                OnPedido.Invoke(this.pedidoEnPreparacion);
            }
        }*/
        private void EsperarProximoIngreso()
        {
            int tiempoEspera = 0;

            while (this.OnDemora is not null && !cancellation.IsCancellationRequested && !this.pedidoEnPreparacion.Estado)
            {
                
                Thread.Sleep(1000);
                tiempoEspera++;
                OnDemora.Invoke(tiempoEspera);
            }

            this.demoraPreparacionTotal += tiempoEspera;

        }

        private void TomarNuevoPedido(T menu)
        {
            if (this.OnPedido is not null)
            {
                this.pedidos.Enqueue(menu);
            }
        }
    }
}
