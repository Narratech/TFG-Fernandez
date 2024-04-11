using UnityEngine;

namespace LiquidSnake.Character
{
    /// <summary>
    /// Delegado para los eventos destinados a notificar de haber alcanzado una región de victoria.
    /// </summary>
    public delegate void WinRegionReachedEvent();

    public class WinRegionReachedHandler : MonoBehaviour, IWinRegionHandler
    {
        /// <summary>
        /// Evento al que suscribirse desde otros scripts para reaccionar ante el suceso
        /// de haber alcanzado la región de victoria por parte del jugador.
        /// </summary>
        public event WinRegionReachedEvent WinRegionReached;

        public void WinRegionEntered()
        {
            if (WinRegionReached != null)
            {
                WinRegionReached();
            }
        }
    }
}
