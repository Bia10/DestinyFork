using System.Collections.Generic;

using Destiny.IO;
using Destiny.Constants;
using Destiny.Network.Common;

namespace Destiny.Maple
{
    public sealed class Movement
    {
        public MapConstants.MovementType Type { get; set; }
        public Point Position { get; set; }
        public Point Velocity { get; set; }
        public short FallStart { get; set; }
        public short Foothold { get; set; }
        public short Duration { get; set; }
        public byte Stance { get; set; }
        public byte Statistic { get; set; }
    }

    public sealed class Movements : List<Movement>
    {
        public static Movements Decode(Packet iPacket)
        {
            return new Movements(iPacket);
        }

        public Point Origin { get; private set; }
        public Point Position { get; private set; }
        public short Foothold { get; private set; }
        public byte Stance { get; private set; }

        public Movements(Packet iPacket)
            : base()
        {
            short foothold = 0;
            byte stance = 0;
            Point position = new Point(iPacket.ReadShort(), iPacket.ReadShort());

            this.Origin = position;

            byte count = iPacket.ReadByte();

            while (count-- > 0)
            {
                MapConstants.MovementType type = (MapConstants.MovementType)iPacket.ReadByte();

                Movement movement = new Movement();

                movement.Type = type;
                movement.Foothold = foothold;
                movement.Position = position;
                movement.Stance = stance;

                switch (type)
                {
                    case MapConstants.MovementType.Normal:
                    case MapConstants.MovementType.Normal2:
                    case MapConstants.MovementType.JumpDown:
                    case MapConstants.MovementType.WingsFalling:
                        {
                            movement.Position = new Point(iPacket.ReadShort(), iPacket.ReadShort());
                            movement.Velocity = new Point(iPacket.ReadShort(), iPacket.ReadShort());
                            movement.Foothold = iPacket.ReadShort();

                            if (movement.Type == MapConstants.MovementType.JumpDown)
                            {
                                movement.FallStart = iPacket.ReadShort();
                            }

                            movement.Stance = iPacket.ReadByte();
                            movement.Duration = iPacket.ReadShort();
                        }
                        break;

                    case MapConstants.MovementType.Jump:
                    case MapConstants.MovementType.JumpKnockback:
                    case MapConstants.MovementType.FlashJump:
                    case MapConstants.MovementType.ExcessiveKnockback:
                    case MapConstants.MovementType.RecoilShot:
                    case MapConstants.MovementType.Aran:
                        {
                            movement.Velocity = new Point(iPacket.ReadShort(), iPacket.ReadShort());
                            movement.Stance = iPacket.ReadByte();
                            movement.Duration = iPacket.ReadShort();
                        }
                        break;

                    case MapConstants.MovementType.Immediate:
                    case MapConstants.MovementType.Teleport:
                    case MapConstants.MovementType.Assaulter:
                    case MapConstants.MovementType.Assassinate:
                    case MapConstants.MovementType.Rush:
                    case MapConstants.MovementType.Chair:
                        {
                            movement.Position = new Point(iPacket.ReadShort(), iPacket.ReadShort());
                            movement.Foothold = iPacket.ReadShort();
                            movement.Stance = iPacket.ReadByte();
                            movement.Duration = iPacket.ReadShort();
                        }
                        break;

                    case MapConstants.MovementType.Falling:
                        {
                            movement.Statistic = iPacket.ReadByte();
                        }
                        break;

                    case MapConstants.MovementType.Unknown:
                        {
                            movement.Velocity = new Point(iPacket.ReadShort(), iPacket.ReadShort());
                            movement.FallStart = iPacket.ReadShort();
                            movement.Stance = iPacket.ReadByte();
                            movement.Duration = iPacket.ReadShort();
                        }
                        break;

                    default:
                        {
                            movement.Stance = iPacket.ReadByte();
                            movement.Duration = iPacket.ReadShort();
                        }
                        break;
                }

                position = movement.Position;
                foothold = movement.Foothold;
                stance = movement.Stance;

                this.Add(movement);
            }

            byte keypadStates = iPacket.ReadByte();

            for (byte i = 0; i < keypadStates; i++)
            {
                if (i % 2 == 0)
                {
                    iPacket.ReadByte(); // NOTE: Unknown.
                }
            }

            // NOTE: Rectangle for bounds checking.
            iPacket.ReadShort(); // NOTE: Left.
            iPacket.ReadShort(); // NOTE: Top.
            iPacket.ReadShort(); // NOTE: Right.
            iPacket.ReadShort(); // NOTE: Bottom.

            this.Position = position;
            this.Stance = stance;
            this.Foothold = foothold;
        }

        public byte[] ToByteArray()
        {
            using (ByteBuffer oPacket = new ByteBuffer())
            {
                oPacket
                    .WriteShort(this.Origin.X)
                    .WriteShort(this.Origin.Y)
                    .WriteByte((byte)this.Count);

                foreach (Movement movement in this)
                {
                    oPacket.WriteByte((byte)movement.Type);

                    switch (movement.Type)
                    {
                        case MapConstants.MovementType.Normal:
                        case MapConstants.MovementType.Normal2:
                        case MapConstants.MovementType.JumpDown:
                        case MapConstants.MovementType.WingsFalling:
                            {
                                oPacket
                                    .WriteShort(movement.Position.X)
                                    .WriteShort(movement.Position.Y)
                                    .WriteShort(movement.Velocity.X)
                                    .WriteShort(movement.Velocity.Y)
                                    .WriteShort(movement.Foothold);

                                if (movement.Type == MapConstants.MovementType.JumpDown)
                                {
                                    oPacket.WriteShort(movement.FallStart);
                                }

                                oPacket
                                    .WriteByte(movement.Stance)
                                    .WriteShort(movement.Duration);
                            }
                            break;

                        case MapConstants.MovementType.Jump:
                        case MapConstants.MovementType.JumpKnockback:
                        case MapConstants.MovementType.FlashJump:
                        case MapConstants.MovementType.ExcessiveKnockback:
                        case MapConstants.MovementType.RecoilShot:
                        case MapConstants.MovementType.Aran:
                            {
                                oPacket
                                    .WriteShort(movement.Velocity.X)
                                    .WriteShort(movement.Velocity.Y)
                                    .WriteByte(movement.Stance)
                                    .WriteShort(movement.Duration);
                            }
                            break;

                        case MapConstants.MovementType.Immediate:
                        case MapConstants.MovementType.Teleport:
                        case MapConstants.MovementType.Assaulter:
                        case MapConstants.MovementType.Assassinate:
                        case MapConstants.MovementType.Rush:
                        case MapConstants.MovementType.Chair:
                            {
                                oPacket
                                    .WriteShort(movement.Position.X)
                                    .WriteShort(movement.Position.Y)
                                    .WriteShort(movement.Foothold)
                                    .WriteByte(movement.Stance)
                                    .WriteShort(movement.Duration);
                            }
                            break;

                        case MapConstants.MovementType.Falling:
                            {
                                oPacket.WriteByte(movement.Statistic);
                            }
                            break;

                        case MapConstants.MovementType.Unknown:
                            {
                                oPacket
                                    .WriteShort(movement.Velocity.X)
                                    .WriteShort(movement.Velocity.Y)
                                    .WriteShort(movement.FallStart)
                                    .WriteByte(movement.Stance)
                                    .WriteShort(movement.Duration);
                            }
                            break;

                        default:
                            {
                                oPacket
                                    .WriteByte(movement.Stance)
                                    .WriteShort(movement.Duration);
                            }
                            break;
                    }
                }

                // NOTE: Keypad and boundary values are not read on the client side.

                oPacket.Flip();
                return oPacket.GetContent();
            }
        }
    }
}
