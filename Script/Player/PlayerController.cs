using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	Rigidbody2D rb;
	Animator animator;

	PlayerStatus playerStatus;
	PlayerSkinChange playerSkinChange;

	Vector3 mousePosition;

	Vector2 inputKeyAxis; //移動キー入力
	[SerializeField] float speedAlpha;
	float speed = 3f;
	float dash;
	float dashSpeed = 1.5f;
	float slowTime = 0f;
	float slowSpeed = 0.7f;
	bool walk;

	float Stamina = 1;
	float StaminaCooltime = 1;
	float StaminaHeal = 0.1f;

	float StaminaInfinity = 0;

	bool Pause = false;

	void Start () {
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();

		playerStatus = GetComponent<PlayerStatus>();
		playerSkinChange = GetComponent<PlayerSkinChange>();

		speed = 3f;
		dashSpeed = 1.5f;

		Stamina = 1;
		StaminaCooltime = 1;
		StaminaHeal = 0.1f;
		StaminaInfinity = 0;
	}
	
	void Update () {
		if (Mathf.Approximately(Time.timeScale, 0f)) return;
		if (PlayerStatus.Die == true) { rb.velocity = Vector2.zero; return; }

		inputKeyAxis.x = Input.GetAxisRaw("Horizontal");// + Input.GetAxisRaw("C_Horizontal");
		inputKeyAxis.y = Input.GetAxisRaw("Vertical");// + Input.GetAxisRaw("C_Vertical");

		StaminaInfinity -= StaminaInfinity > 0 ? Time.deltaTime : 0;
		slowTime -= slowTime > 0 ? Time.deltaTime : 0;
		if (slowTime > 0) playerSkinChange.PlayerColor(new Color32(127, 255, 127, 255));
		else playerSkinChange.PlayerColor(new Color32(255, 255, 255, 255));

		WayChange();

		if ((Input.GetKey(KeyConfig.Dash) || Input.GetButton("C_Dash")) && ((Stamina > 0 && inputKeyAxis != Vector2.zero) || (StaminaInfinity > 0)))
		{
			dash = dashSpeed + speedAlpha;
			if (StaminaInfinity <= 0 && speedAlpha == 0)
			{
				StaminaCooltime = 1.5f;
				Stamina -= 0.50f * Time.deltaTime;
				Stamina = Stamina < 0 ? 0 : Stamina;
			}
		}
		else
		{
			dash = 1;
			StaminaCooltime = StaminaCooltime > 0 ? StaminaCooltime - Time.deltaTime : 0;
		}

		if (StaminaCooltime <= 0)
		{
			Stamina = Stamina < 1 ? Stamina + (StaminaHeal * Time.deltaTime) : 1;
		}

		playerStatus.StaminaBarChange(Stamina);

		rb.velocity = inputKeyAxis * speed * (dash + Artifact.GetArtifactAmount(7)) * (slowTime > 0 ? slowSpeed : 1f);
	}

	void WayChange()
	{
		MousePositionGet();
		Vector2 Axis = TragetCursor.MousePosion - transform.position;
		Axis = Axis.normalized;

		walk = inputKeyAxis != Vector2.zero ? true : false;
		animator.SetBool("Walk", walk);
		animator.SetFloat("Direction_X", Axis.x);
		animator.SetFloat("Direction_Y", Axis.y);
	}

	void MousePositionGet()
	{
		Vector3 ScreenWorldMousePosition = Input.mousePosition;
		ScreenWorldMousePosition.z = 10f;
		mousePosition = Camera.main.ScreenToWorldPoint(ScreenWorldMousePosition);
	}

	public void BoostDash()
	{
		StaminaHeal = 0.142857f;
		dashSpeed = 1.75f;
	}

	public void SetStaminaInfinity(float time)
	{
		StaminaInfinity = time;
	}

	public void Slowness(float time)
	{
		slowTime = time;
	}
}
