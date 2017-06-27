using System.Collections.Generic;

using Android.App;
using Android.Views;
using Android.Widget;
using HackAtHome.Entities;

namespace HackAtHome.CustomAdapters
{
    public class EvidenceAdapter : BaseAdapter<Evidence>
    {
        List<Evidence> Items; // Datos de cada evidencia de laboratorio
        Activity Context; // Activity donde se utilizara este adapter
        int ItemLayoutTemplate; // Layout a utilizar para mostrar los datos de un elemento
        int EvidenceTitleViewID; // ID del TextView donde se mostrara el nombre de la evidencia
        int EvidenceStatusViewID; // ID del TextView donde se mostrara el estatus de la evidencia
        
        /// <summary>
        /// Constructor para recibir la informacion que necesita el adapter
        /// </summary>
        /// <param name="context">Activity donde se aloja el ListView</param>
        /// <param name="evidences">La lista de elementos</param>
        /// <param name="itemLayoutTemplate">ID del Layout para mostrar cada elemento del ListView</param>
        /// <param name="evidenceTitleViewID">ID del TextView donde se mostrara el titulo de la evidencia</param>
        /// <param name="evidenceStatusViewID">ID del TextView donde se mostrara el estatus de la evidencia</param>
        public EvidenceAdapter(Activity context, List<Evidence> evidences, int itemLayoutTemplate, int evidenceTitleViewID, int evidenceStatusViewID)
        {
            Context = context;
            Items = evidences;
            ItemLayoutTemplate = itemLayoutTemplate;
            EvidenceTitleViewID = evidenceTitleViewID;
            EvidenceStatusViewID = evidenceStatusViewID;
        }

        /// <summary>
        /// Devuelve el elemento de la lista localizado en la posicion especificada
        /// </summary>
        /// <param name="position">Posicion del elemento dentro de la lista</param>
        /// <returns></returns>
        public override Evidence this[int position] {
            get
            {
                return Items[position];
            }
        }

        /// <summary>
        /// Devuelve el numero de elementos en la lista
        /// </summary>
        public override int Count
        {
            get
            {
                return Items == null ? 0 : Items.Count;
            }
        }

        /// <summary>
        /// Devuelve el ID del elemento localizado en la posicion especificada.
        /// </summary>
        /// <param name="position">Posicion del elemento dentro de la lista</param>
        /// <returns></returns>
        public override long GetItemId(int position)
        {
            return Items[position].EvidenceID;
        }

        /// <summary>
        /// Devuelve el View que muestra los datos de un elemento del conjunto de datos.
        /// </summary>
        /// <param name="position">Posicion del elemento a mostrar.</param>
        /// <param name="convertView">View anterior que puede ser reutilizada.</param>
        /// <param name="parent">View padre al que podria ajustarse el view devuelto.</param>
        /// <returns></returns>
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            // Obtenemos el elemento del cual se requiere la vista
            var Item = Items[position];
            View ItemView; // Vista que vamos a devolver

            if(convertView == null)
            {
                // No hay vista reutilizable, crear una nueva
                ItemView = Context.LayoutInflater.Inflate(ItemLayoutTemplate, null /* No hay view padre */);
            }
            else
            {
                // Reutilizamos un View existente para ahorrar recursos
                ItemView = convertView;
            }

            // Establecemos los datos del elemento dentro del view
            ItemView.FindViewById<TextView>(EvidenceTitleViewID).Text = Item.Title;
            ItemView.FindViewById<TextView>(EvidenceStatusViewID).Text = Item.Status;

            return ItemView;
        }
    }
}