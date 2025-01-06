using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevelopment_SchoofsYmke.Animation
{
    public enum AnimationState { Moving, Jumping, Idle, Attack1, Attack2, Attack3, Hit, Dead }
    public class Animatie
    {
        public AnimationFrame CurrentFrame { get; set; }
        private Dictionary<AnimationState, (int startFrame, int frameCount)> animations;
        private List<AnimationFrame> allFrames;
        private AnimationState currentState;
        public AnimationState CurrentState => currentState;
        private int currentRow;
        private int framesPerRow;
        private int extraFrames;
        private int counter;
        private double secondCounter = 0;

        public Animatie()
        {
            animations = new Dictionary<AnimationState, (int startFrame, int frameCount)>();
            allFrames = new List<AnimationFrame>();
            CurrentFrame = null;
        }
        public void SetAnimationState(AnimationState state)
        {
            if (currentState != state)
            {
                currentState = state;
                counter = 0; 
            }
        }

        public void Update(GameTime gameTime)
        {
            var (startFrame, frameCount) = animations[currentState];
            CurrentFrame = allFrames[startFrame + (counter % frameCount)]; 

            secondCounter += gameTime.ElapsedGameTime.TotalSeconds;
            int fps = 12;

            if (secondCounter >= 1d / fps)
            {
                counter++;
                secondCounter = 0;
            }

            if (counter >= frameCount)
            {
                counter = 0;
            }
        }

        public void GetFramesFromTexture
            (int width, int height, int numberOfWidthSprites, int numberOfHeightsprites, string characterType)
        {
            allFrames.Clear();

            int widthOfFrame = width / numberOfWidthSprites;
            int heightOfFrame = height / numberOfHeightsprites;

            for (int y = 0; y < height; y += heightOfFrame)
            {
                for (int x = 0; x < width; x += widthOfFrame)
                {
                     allFrames.Add(new AnimationFrame(new Rectangle(x, y, widthOfFrame, heightOfFrame)));
                }
            }
            Debug.WriteLine($"Total frames added: {allFrames.Count} + {characterType}");
            CurrentFrame = allFrames.FirstOrDefault();

            switch (characterType)
            {
                case "hero":
                    animations[AnimationState.Moving] = (13, 9);
                    animations[AnimationState.Idle] = (0, 8);
                    animations[AnimationState.Jumping] = (65, 12);
                    animations[AnimationState.Hit] = (77, 3);
                    animations[AnimationState.Attack1] = (39, 8);
                    break;
                case "loader":
                    animations[AnimationState.Moving] = (48, 4);
                    animations[AnimationState.Idle] = (36, 4);
                    animations[AnimationState.Attack1] = (0, 6);
                    animations[AnimationState.Hit] = (30, 2);
                    break;
                case "security":
                    animations[AnimationState.Idle] = (48, 4);
                    animations[AnimationState.Moving] = (64, 6);
                    animations[AnimationState.Hit] = (40, 2);
                    animations[AnimationState.Attack1] = (23, 6) ;
                    break;
                case "operator":
                    animations[AnimationState.Idle] = (48, 4);
                    animations[AnimationState.Moving] = (72, 8);
                    animations[AnimationState.Hit] = (40, 2);
                    animations[AnimationState.Attack1] = (32, 4);
                    break;
                case "projectile":
                    animations[AnimationState.Idle] = (0, 6);
                    break;
                case "healthbar":
                    animations[AnimationState.Idle] = (0, 1);
                    animations[AnimationState.Hit] = (0, 4);
                    break;
                case "boom":
                    animations[AnimationState.Idle] = (0, 8);
                    break;
            }
        }
        public bool IsAnimationComplete()
        {
            var (_, frameCount) = animations[currentState];
            return counter >= frameCount;
        }



    }

}
