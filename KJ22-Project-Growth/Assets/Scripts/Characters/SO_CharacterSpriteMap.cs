using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu()]
public class SO_CharacterSpriteMap : ScriptableObject
{
	[Header("Default")]
	[HorizontalLine(1, EColor.Black)]
	[SerializeField]
	private Sprite m_DefaultHeartTexture;

	[Header("Player 1")]
	[HorizontalLine(1, EColor.Green)]
	[SerializeField]
	private Sprite m_player1Texture;
	[SerializeField]
	private Sprite m_player1BlobTexture;
	[SerializeField]
	private Sprite m_player1HeartTexture;

	[Header("Player 2")]
	[HorizontalLine(1, EColor.Pink)]
	[SerializeField]
	private Sprite m_player2Texture;
	[SerializeField]
	private Sprite m_player2BlobTexture;
	[SerializeField]
	private Sprite m_player2HeartTexture;

	[Header("Player 3")]
	[HorizontalLine(1, EColor.Red)]
	[SerializeField]
	private Sprite m_player3Texture;
	[SerializeField]
	private Sprite m_player3BlobTexture;
	[SerializeField]
	private Sprite m_player3HeartTexture;

	[Header("Player 4")]
	[HorizontalLine(1, EColor.Orange)]
	[SerializeField]
	private Sprite m_player4Texture;
	[SerializeField]
	private Sprite m_player4BlobTexture;
	[SerializeField]
	private Sprite m_player4HeartTexture;
	
	public Sprite GetPlayerSprite(EPlayerOwnership owner)
	{
		switch(owner)
		{
		case	EPlayerOwnership.AI:
			return null;
			
		case	EPlayerOwnership.Player1:
			return m_player1Texture;
			
		case	EPlayerOwnership.Player2:
			return m_player2Texture;
			
		case	EPlayerOwnership.Player3:
			return m_player3Texture;
			
		case	EPlayerOwnership.Player4:
			return m_player4Texture;
			
		default:
			return null;
		}
	}
	
	public Sprite GetPlayerBulletSprite(EPlayerOwnership owner)
	{
		switch(owner)
		{
		case	EPlayerOwnership.AI:
			return null;
			
		case	EPlayerOwnership.Player1:
			return m_player1BlobTexture;
			
		case	EPlayerOwnership.Player2:
			return m_player2BlobTexture;
			
		case	EPlayerOwnership.Player3:
			return m_player3BlobTexture;
			
		case	EPlayerOwnership.Player4:
			return m_player4BlobTexture;
			
		default:
			return null;
		}
	}
	
	public Sprite GetHeartSprite(EPlayerOwnership owner)
	{
		switch(owner)
		{
		case	EPlayerOwnership.Player1:
			return m_player1HeartTexture;
			
		case	EPlayerOwnership.Player2:
			return m_player2HeartTexture;
			
		case	EPlayerOwnership.Player3:
			return m_player3HeartTexture;
			
		case	EPlayerOwnership.Player4:
			return m_player4HeartTexture;
			
		default:
			return m_DefaultHeartTexture;
		}
	}
}
