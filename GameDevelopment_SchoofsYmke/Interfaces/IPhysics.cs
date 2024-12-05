using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevelopment_SchoofsYmke.Interfaces
{
    internal interface IPhysics
    {
        float ApplyGravity(float verticalVelocity, float deltaTime);
        float Jump(float jumpForce);
    }
}
