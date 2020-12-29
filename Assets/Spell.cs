using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// functionality of spell
// animates color and size of the spell
// and attacks the player if the player is near the spell (check the code)
public class Spell : MonoBehaviour
{
    private GameObject fps_player_obj;
    private Level level;
    private float radius_of_search_for_player;
    private float spell_speed;

	void Start ()
    {
        GameObject level_obj = GameObject.FindGameObjectWithTag("Level");
        level = level_obj.GetComponent<Level>();
        if (level == null)
        {
            Debug.LogError("Internal error: could not find the Level object - did you remove its 'Level' tag?");
            return;
        }
        
        fps_player_obj = level.fps_player_obj;
        Bounds bounds = level.GetComponent<Collider>().bounds;
        radius_of_search_for_player = (bounds.size.x + bounds.size.z) / 10.0f;
        spell_speed = level.spell_speed;
    }

    // *** YOU NEED TO COMPLETE THIS PART OF THE FUNCTION TO ANIMATE THE spell ***
    // so that it moves towards the player when the player is within radius_of_search_for_player
    // a simple strategy is to update the position of the spell
    // so that it moves towards the direction d=v/||v||, where v=(fps_player_obj.transform.position - transform.position)
    // with rate of change (spell_speed * Time.deltaTime)
    // make also sure that the spell y-coordinate position does not go above the wall height
    void Update()
    {
        if (level.player_health < 0.001f || level.player_entered_house)
            return;
        Color redness = new Color
        {
            r = Mathf.Max(1.0f, 0.25f + Mathf.Abs(Mathf.Sin(2.0f * Time.time)))
        };
        if ( transform.childCount > 0)
            transform.GetChild(0).GetComponent<MeshRenderer>().material.color = redness;
        else
            transform.GetComponent<MeshRenderer>().material.color = redness;
        transform.localScale = new Vector3(
                               0.9f + 0.2f * Mathf.Abs(Mathf.Sin(4.0f * Time.time)), 
                               0.9f + 0.2f * Mathf.Abs(Mathf.Sin(4.0f * Time.time)), 
                               0.9f + 0.2f * Mathf.Abs(Mathf.Sin(4.0f * Time.time))
                               );
        if(fps_player_obj==null)
            {fps_player_obj = level.fps_player_obj;}
        if( (fps_player_obj.transform.position - transform.position).magnitude < radius_of_search_for_player){
            Vector3 direction = (fps_player_obj.transform.position - transform.position).normalized;
            transform.position +=  direction * spell_speed * Time.deltaTime / 10.0f;
            transform.position = new Vector3(transform.position.x, 1.0f, transform.position.z);


        }


    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "PLAYER")
        {
            level.myaudio.PlayOneShot(level.collide);
            
            if (!level.spell_landed_on_player_recently)
                level.timestamp_spell_landed = Time.time;
            level.num_spell_hit_concurrently++;
            level.spell_landed_on_player_recently = true;
            Destroy(gameObject);
        }
    }
    
}
