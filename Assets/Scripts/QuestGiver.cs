using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    [SerializeField] private GameObject textPopupStart;
    [SerializeField] private GameObject textPopupComplete;
    [SerializeField] private GameObject stonePlatformOne;
    [SerializeField] private GameObject stonePlatformTwo;

    // Gör referens till QSP klass
    [SerializeField] private QuestStonePlatform questStonePlatformOne, questStonePlatformTwo;

    private bool questOneComplete = false;
    private bool platformStart = false;


    private void OnTriggerEnter2D(Collider2D other)
    {

        // Kollar om första questet är klart
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<PlayerQuest>().GetBananas() >= other.GetComponent<PlayerQuest>().GetBananasToCollect())
            {
                questOneComplete = true;

                platformStart = true;
            }

            // Startar platformar till nästa area
            if (platformStart)
            {   
                stonePlatformOne.SetActive(true);
                stonePlatformTwo.SetActive(true);

                questStonePlatformOne.questOneCompletePlatform = true;
                questStonePlatformTwo.questOneCompletePlatform = true;
            }
        }

        // Visar samma text om questet inte är klart
        if (other.CompareTag("Player") && (!questOneComplete))
        {
            textPopupStart.SetActive(true);
        }

        // Visar quest complete text 
        if (other.CompareTag("Player") && (questOneComplete))
        {
            textPopupComplete.SetActive(true);
        }
    }

    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (textPopupStart)
            {
                textPopupStart.SetActive(false);
            }
            
            if (textPopupComplete)
            {
                textPopupComplete.SetActive(false);
            }
        }
    }
}
