using System.Collections;
using UnityEngine;
using TMPro;

public class CubeGenerator : MonoBehaviour
{
    public GameObject cubePrefab;
    public TMP_InputField inputField;
    public float spacing = 2f;
    public Color textColor = Color.white;
    public float sortingDelay = 1f; // Delay before starting the sorting process

    private GameObject[] cubes;
    private bool sortingInProgress = false;

    private void Start()
    {
        // You can add a listener to the submit button or call GenerateCubes() from elsewhere in your code.
    }

    public void GenerateCubes()
    {
        if (sortingInProgress)
        {
            // If sorting is already in progress, ignore the button click
            return;
        }

        sortingInProgress = true; // Set flag to indicate sorting is in progress

        if (cubes != null)
        {
            // Clean up previously generated cubes
            foreach (GameObject cube in cubes)
            {
                Destroy(cube);
            }
        }

        string Nos = inputField.text;
        string[] numbers = Nos.Split(',');

        Debug.Log("Number of elements in numbers array: " + numbers.Length); // Debug print

        // Calculate total width
        float totalWidth = (numbers.Length - 1) * spacing;

        // Calculate starting position
        float startX = -totalWidth / 2f;

        // Initialize currentX to starting position
        float currentX = startX;

        cubes = new GameObject[numbers.Length]; // Initialize the array to store cube references

        for (int i = 0; i < numbers.Length; i++)
        {
            // Use the current position for each cube
            Vector3 cubePosition = new Vector3(currentX, 0f, 0f);

            Debug.Log("Position of cube " + (i + 1) + ": " + cubePosition); // Debug print

            GameObject cube = Instantiate(cubePrefab, cubePosition, Quaternion.identity);

            // Update currentX for the next cube
            currentX += spacing * 2; // Double the spacing to ensure even spacing

            cubes[i] = cube; // Store reference to the cube in the array

            // Access the TextMeshPro component inside the canvas of the cube prefab and update its text
            Canvas canvas = cube.GetComponentInChildren<Canvas>();
            if (canvas != null)
            {
                TextMeshProUGUI textMesh = canvas.GetComponentInChildren<TextMeshProUGUI>();
                if (textMesh != null)
                {
                    textMesh.text = numbers[i];
                    textMesh.color = textColor;
                    textMesh.alignment = TextAlignmentOptions.Center;

                    // Set font size based on cube size
                    float cubeSize = 24.2f; // Adjust this value based on your cube size
                    float fontSizeMultiplier = 0.05f; // Adjust this multiplier as needed
                    textMesh.fontSize = Mathf.RoundToInt(cubeSize * fontSizeMultiplier);
                }
                else
                {
                    Debug.LogError("TextMeshProUGUI component not found in the canvas of the cube prefab.");
                }
            }
            else
            {
                Debug.LogError("Canvas component not found in the children of the cube prefab.");
            }
        }

        StartCoroutine(BubbleSortCoroutine());
    }

    private IEnumerator BubbleSortCoroutine()
    {
        yield return new WaitForSeconds(sortingDelay);

        int n = cubes.Length;
        bool swapped;

        do
        {
            swapped = false;
            for (int i = 1; i < n; i++)
            {
                // Compare adjacent cubes and swap if necessary
                int currentValue = int.Parse(cubes[i].GetComponentInChildren<TextMeshProUGUI>().text);
                int previousValue = int.Parse(cubes[i - 1].GetComponentInChildren<TextMeshProUGUI>().text);

                if (currentValue < previousValue)
                {
                    // Swap cubes
                    Vector3 tempPosition = cubes[i].transform.position;
                    cubes[i].transform.position = cubes[i - 1].transform.position;
                    cubes[i - 1].transform.position = tempPosition;

                    GameObject tempCube = cubes[i];
                    cubes[i] = cubes[i - 1];
                    cubes[i - 1] = tempCube;

                    swapped = true;
                }

                yield return new WaitForSeconds(1f); // Adjust the delay as needed for visualization
            }
            n--;
        } while (swapped);

        sortingInProgress = false; // Sorting is complete
    }
}
