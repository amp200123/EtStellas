﻿using Microsoft.Xna.Framework;

namespace BPA_RPG.Screens
{
    public class Camera
    {
        public Matrix transform;
        Vector2 center;
        Game game;

        public Camera(Game game)
        {
            this.game = game;
        }

        public void Update(Vector2 position)
        {
            center = new Vector2(position.X - MainGame.WindowWidth / 4, position.Y - MainGame.WindowHeight / 2);
            transform = Matrix.CreateScale(new Vector3(1, 1, 0)) *
                Matrix.CreateTranslation(new Vector3(-center.X, -center.Y, 0));
        }
    }
}