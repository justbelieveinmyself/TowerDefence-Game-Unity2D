using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using TMPro;
public class UnitFactory : MonoBehaviour
{
    public int wave = 0;
    public TMP_Text WaveTextNum;
    public int totalNumberOfUnits;
    private int waveNumberOfUnits = 1;
    public GameObject ogre;
    public static int currentCountOfUnits = 0;
    private int dispersion = 3;
    public void CreateUnit(GameObject Unit) 
    {
        Enemy enemy = Instantiate(Unit.GetComponent<Enemy>());
        enemy.SpawnTo(new Vector2(5,2.5f));

    }
    private void Start() 
    {
        SpawnNewWave(ogre, waveNumberOfUnits, 1000);
    }
    private void Update() {
        WaveTextNum.text = wave.ToString();
        if(currentCountOfUnits == 0)
        {
            waveNumberOfUnits += Random.Range(1, dispersion);
            SpawnNewWave(ogre, waveNumberOfUnits, 1000);
        }
    }
    public async void SpawnNewWave(GameObject Unit, int startNumberOfUnits, int intervalInMilliSec)
    {
        wave++;
        if(wave % 5 == 0)
        {
            Unit.GetComponent<Enemy>().Upgrade(wave);
        }
        totalNumberOfUnits += startNumberOfUnits;
        for(int i = 0; i < startNumberOfUnits + 3 * wave; i++)
        {
            CreateUnit(Unit);
            currentCountOfUnits++;
            await Task.Delay(intervalInMilliSec);
        }
    }
}
