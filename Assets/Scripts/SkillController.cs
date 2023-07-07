using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SkillController : MonoBehaviour
{
   public List<Skills> availableSkills;
    public PlayerBehavior player;
    public Skills WALLJUMP;
    private GameManager gm;

    void Start()
    {
        availableSkills = new List<Skills>();
        player = gameObject.GetComponent<PlayerBehavior>();

        gm = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>();
    }

    public bool hasSkill(Skills s)
    {
        return availableSkills.IndexOf(s) != -1;
    }

    public void obtainSkill(Skills s)
    {
         availableSkills.Add(s);
        if (availableSkills.Count > 0) gm.toggleTextIcon();
        if (s == Skills.DASH) gm.toggleDashIcon();
        if (s == Skills.SLASH) gm.toggleSlashIcon();
        if (s == Skills.WALLJUMP) gm.toggleWJIcon();
    }

    public void dash()
    {
        player.speed = player.runSpeed;   
    }


}



