using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FuelManager : MonoBehaviour
{
    public TMP_Text FuelText;
    public Image FuelImage;
    private float fuel = 100.0f;
    private int blocksMoved = 0;

    private Bike _bike;

    private void Start()
    {
        // Find the Bike component
        _bike = FindObjectOfType<Bike>();
        if (_bike != null)
        {
            _bike.OnPositionChanged += UpdateFuel;
            _bike.IsOutOfFuel = false; // Restablecer IsOutOfFuel a false
        }
        else
        {
            Debug.LogError("Bike component not found in the scene.");
        }
    }

    private void OnDestroy()
    {
        if (_bike != null)
        {
            _bike.OnPositionChanged -= UpdateFuel;
        }
    }

    public void Initialize(TMP_Text fuelText, Image fuelImage)
    {
        FuelText = fuelText;
        FuelImage = fuelImage;
    }

    public void UpdateFuel(Vector2 currentPosition, Vector2 previousPosition)
    {
        if (currentPosition != previousPosition)
        {
            blocksMoved++;

            // Cada 5 bloques, reducir el combustible en un 1%
            if (blocksMoved % 5 == 0)
            {
                fuel -= 1.0f;
                if (fuel < 0) fuel = 0; // Asegurarse de que el combustible no sea negativo
            }

            UpdateUI();

            // Enviar el booleano a la clase Bike si el combustible llega a 0
            if (fuel == 0 && _bike != null)
            {
                _bike.IsOutOfFuel = true;
            }
        }
    }

    private void UpdateUI()
    {
        if (FuelImage != null)
        {
            FuelImage.fillAmount = fuel / 100.0f;
        }

        if (FuelText != null)
        {
            FuelText.text = "Current Fuel = " + fuel + "%";
        }
    }
}