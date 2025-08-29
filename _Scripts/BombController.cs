using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class BombController : MonoBehaviour
{
    [Header("Bomb")]
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private KeyCode placeBomb = KeyCode.Space;
    [SerializeField] private int bombAmount;
    [SerializeField] private int bombsRemaining;
    private HashSet<Vector2> activeBombPositions = new HashSet<Vector2>();
    public float bombFuseTime;

    [Header("Explosion")]
    [SerializeField] private Explosion explosionPrefab;
    [SerializeField] private float explosionDuration;
    [SerializeField] private LayerMask explosionLayerMask;
    public int explosionRadius;

    [Header("Destructible")]
    [SerializeField] private Destructible destructiblePrefab;
    [SerializeField] private Tilemap destructibleTiles;

    public ItemPickUp item;

    private void OnEnable()
    {
        bombsRemaining = bombAmount;
    }

    private void Update()
    {
        if (bombsRemaining > 0 && Input.GetKeyDown(placeBomb))
        {
            StartCoroutine(PlaceBomb());
        }
    }

    private IEnumerator PlaceBomb()
    {
        Vector2 position = transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        // Không cho đặt bom chồng bom
        if (activeBombPositions.Contains(position))
            yield break;

        // Đánh dấu đã có bom tại vị trí này
        activeBombPositions.Add(position);

        GameObject bomb = Instantiate(bombPrefab, position, Quaternion.identity);
        bombsRemaining--;

        yield return new WaitForSeconds(bombFuseTime);

        // Làm tròn lại vị trí để chắc chắn
        position = bomb.transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        // Tạo vụ nổ trung tâm
        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        explosion.SetActiveRenderer(explosion.start);
        explosion.DestroyAfter(explosionDuration);

        // Tạo vụ nổ theo 4 hướng
        Explode(position, Vector2.up, explosionRadius);
        Explode(position, Vector2.down, explosionRadius);
        Explode(position, Vector2.left, explosionRadius);
        Explode(position, Vector2.right, explosionRadius);

        // Hủy bom và cập nhật lại số lượng
        Destroy(bomb);
        bombsRemaining++;

        // Gỡ vị trí bom ra khỏi danh sách
        activeBombPositions.Remove(position);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Bomb"))
        {
            collision.isTrigger = false;
        }
    }

    private void Explode(Vector2 position, Vector2 direction, int length)
    {
        if (length <= 0)
        {
            return;
        }

        position += direction;

        if (Physics2D.OverlapBox(position, Vector2.one / 2f, 0f, explosionLayerMask))
        {
            ClearDestructible(position);
            return;
        }

        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        explosion.SetActiveRenderer(length > 1 ? explosion.middle : explosion.end);
        explosion.SetDirection(direction);
        explosion.DestroyAfter(explosionDuration);

        Explode(position, direction, length - 1);
    }

    private void SpawnExplosion(Vector2 position, Vector2 direction, int length)
    {
        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        explosion.SetActiveRenderer(length > 1 ? explosion.middle : explosion.end);
        explosion.SetDirection(direction);
        explosion.DestroyAfter(explosionDuration);
    }

    private void ClearDestructible(Vector2 position)
    {
        Vector3Int cell = destructibleTiles.WorldToCell(position);
        TileBase tile = destructibleTiles.GetTile(cell);

        if (tile != null)
        {
            Instantiate(destructiblePrefab, position, Quaternion.identity);
            destructibleTiles.SetTile(cell, null);
        }
    }

    public void AddBomb()
    {
        bombAmount++;
        bombsRemaining++;
    }

    public void AddTime()
    {
        bombFuseTime += 0.5f;
    }

    public void AddExplosionRadius()
    {
        explosionRadius += 1;
    }
}