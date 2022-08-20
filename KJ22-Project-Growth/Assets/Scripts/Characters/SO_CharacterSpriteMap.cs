using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class SO_CharacterSpriteMap : ScriptableObject
{
	[SerializeField]
	private Sprite m_player1Texture;
	public Sprite Player1Texture => m_player1Texture;
	[SerializeField]
	private Sprite m_player1BlobTexture;
	public Sprite Player1BlobTexture => m_player1BlobTexture;
	
	[SerializeField]
	private Sprite m_player2Texture;
	public Sprite Player2Texture => m_player2Texture;
	[SerializeField]
	private Sprite m_player2BlobTexture;
	public Sprite Player2BlobTexture => m_player2BlobTexture;
	
	[SerializeField]
	private Sprite m_player3Texture;
	public Sprite Player3Texture => m_player3Texture;
	[SerializeField]
	private Sprite m_player3BlobTexture;
	public Sprite Player3BlobTexture => m_player3BlobTexture;
	
	[SerializeField]
	private Sprite m_player4Texture;
	public Sprite Player4Texture => m_player4Texture;
	[SerializeField]
	private Sprite m_player4BlobTexture;
	public Sprite Player4BlobTexture => m_player4BlobTexture;
	
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
}
