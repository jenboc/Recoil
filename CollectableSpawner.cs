using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Recoil
{
    class Collectable
    {
        public SpriteClass sprite;
        public int value;
    }

    abstract class CollectableSpawner
    { 
        public Texture2D CollectableTexture { get; set; }
        public List<Collectable> Collectables { get; set; }
        public float xBoundary { get; set; }
        public float yBoundary { get; set; }
        Random r { get; set; }
        public int maxSpawn { get; set; }

        public Collectable this[int index]
        {
            get { return Collectables[index]; }
        }

        public CollectableSpawner(ContentManager Content, string textureName, float yBoundary, float xBoundary) 
        {
            CollectableTexture = Content.Load<Texture2D>(textureName);
            Collectables = new List<Collectable>();
            this.xBoundary = xBoundary;
            this.yBoundary = yBoundary;
            r = new Random();
        }

        public void Spawn(GraphicsDevice gDevice, int amount=1)
        {
            if (Collectables.Count > maxSpawn) return;
            float x = r.Next(0,(int)xBoundary);
            float y = r.Next(0, (int)yBoundary);
            int value = r.Next(1, 5);

            Collectable newCollectable = new Collectable();
            newCollectable.sprite = new SpriteClass(gDevice, CollectableTexture, 1f);
            newCollectable.sprite.x = x;
            newCollectable.sprite.y = y;
            newCollectable.value = value;

            Collectables.Add(newCollectable);

            amount--;
            if (amount > 0) Spawn(gDevice, amount);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Collectables.Count == 0) return;
            foreach (Collectable c in Collectables)
            {
                c.sprite.Draw(spriteBatch);
            }
        }

        public void CheckCollisions(Player player)
        {
            for (int i = 0; i < Collectables.Count; i++)
            {
                Collectable c = Collectables[i];
                if (player.RectangleCollision(c.sprite)
                    || player.Arms.RectangleCollision(c.sprite)
                    || player.Head.RectangleCollision(c.sprite)
                    || player.Gun.RectangleCollision(c.sprite)) CollectableEffect(player, i);
            }
        }

        public void DestroyAll()
        {
            Collectables.Clear();
        }

        public abstract void CollectableEffect(Player player, int cIndex);
    }

    class AmmoSpawner : CollectableSpawner {
        public int ammoPerCollectable { get; set; }        
        public AmmoSpawner(ContentManager Content, string textureName, float yBoundary, float xBoundary) : base(Content, textureName, yBoundary, xBoundary)
        {
            ammoPerCollectable = 1;
            maxSpawn = 5;
        }

        public override void CollectableEffect(Player player, int cIndex)
        {
            player.AddAmmo(Collectables[cIndex].value);
            Collectables.RemoveAt(cIndex);
        }
    }
}
