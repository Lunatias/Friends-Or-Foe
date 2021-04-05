using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    Unit playerUnit;
    Unit enemyUnit;

    public Text dialogueText;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;



    public BattleState state;

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetUpBattle());
    }

   IEnumerator SetUpBattle()
    {

        GameObject playerGo = Instantiate(playerPrefab, playerBattleStation);
        playerUnit = playerGo.GetComponent<Unit>();

        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnit = enemyGO.GetComponent<Unit>();

        dialogueText.text = "This " + enemyUnit.unitName + " seems shy...";

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        yield return new WaitForSeconds(2f);



        state = BattleState.PLAYERTURN;
        PlayerTurn();

    }

    IEnumerator PlayerAttack()
    {
        bool PassedOut = enemyUnit.TakeDamage(playerUnit.damage);

        enemyHUD.SetHP(enemyUnit.currentHP);
        dialogueText.text = "Hey! That was Painful!";

        yield return new WaitForSeconds(2f);


        if (PassedOut)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
        
    }

    IEnumerator EnemyTurn()
    {
        dialogueText.text = enemyUnit.unitName + " is shocked!";

        yield return new WaitForSeconds(1f);

        bool PassedOut = playerUnit.TakeDamage(enemyUnit.damage);

        playerHUD.SetHP(playerUnit.currentHP);

        yield return new WaitForSeconds(1f);

        if (PassedOut)
        {
            state = BattleState.LOST;
            EndBattle();
        }else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
        
    }

    void EndBattle()
    {
        if(state == BattleState.LOST)
        {
            dialogueText.text = "That's wasn't nice...";
        } else if (state == BattleState.WON)
        {
            dialogueText.text = "A new friend!";
        }
        
    }

    void PlayerTurn()
    {
        dialogueText.text = "What will you do?";
    }

    IEnumerator PlayerNice()
    {
        playerUnit.Nice(5);

        playerHUD.SetHP(playerUnit.currentHP);
        dialogueText.text = "Ah, they seem friendly...";

        yield return new WaitForSeconds(2f);
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
        return;

        StartCoroutine(PlayerAttack());
    }


    public void OnShakeHandButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerNice());
    }
}
