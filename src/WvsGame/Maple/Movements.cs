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
        public static Movements Decode(Packet inPacket)
        {
            return new Movements(inPacket);
        }

        public Point Origin { get; private set; }
        public Point Position { get; private set; }
        public short Foothold { get; private set; }
        public byte Stance { get; private set; }

        public Movements(Packet inPacket) : base()
        {
            short foothold = 0;
            byte stance = 0;
            Point position = new Point(inPacket.ReadShort(), inPacket.ReadShort());

            Origin = position;

            byte count = inPacket.ReadByte();

            while (count-- > 0)
            {
                MapConstants.MovementType type = (MapConstants.MovementType)inPacket.ReadByte();

                Movement movement = new Movement
                {
                    Type = type,
                    Foothold = foothold,
                    Position = position,
                    Stance = stance
                };

                switch (type)
                {
                    case MapConstants.MovementType.Normal:
                    case MapConstants.MovementType.Normal2:
                    case MapConstants.MovementType.JumpDown:
                    case MapConstants.MovementType.WingsFalling:
                        {
                            movement.Position = new Point(inPacket.ReadShort(), inPacket.ReadShort());
                            movement.Velocity = new Point(inPacket.ReadShort(), inPacket.ReadShort());
                            movement.Foothold = inPacket.ReadShort();

                            if (movement.Type == MapConstants.MovementType.JumpDown)
                            {
                                movement.FallStart = inPacket.ReadShort();
                            }

                            movement.Stance = inPacket.ReadByte();
                            movement.Duration = inPacket.ReadShort();
                        }
                        break;

                    case MapConstants.MovementType.Jump:
                    case MapConstants.MovementType.JumpKnockback:
                    case MapConstants.MovementType.FlashJump:
                    case MapConstants.MovementType.ExcessiveKnockback:
                    case MapConstants.MovementType.RecoilShot:
                    case MapConstants.MovementType.Aran:
                        {
                            movement.Velocity = new Point(inPacket.ReadShort(), inPacket.ReadShort());
                            movement.Stance = inPacket.ReadByte();
                            movement.Duration = inPacket.ReadShort();
                        }
                        break;

                    case MapConstants.MovementType.Immediate:
                    case MapConstants.MovementType.Teleport:
                    case MapConstants.MovementType.Assaulter:
                    case MapConstants.MovementType.Assassinate:
                    case MapConstants.MovementType.Rush:
                    case MapConstants.MovementType.Chair:
                        {
                            movement.Position = new Point(inPacket.ReadShort(), inPacket.ReadShort());
                            movement.Foothold = inPacket.ReadShort();
                            movement.Stance = inPacket.ReadByte();
                            movement.Duration = inPacket.ReadShort();
                        }
                        break;

                    case MapConstants.MovementType.Falling:
                        {
                            movement.Statistic = inPacket.ReadByte();
                        }
                        break;

                    case MapConstants.MovementType.Unknown:
                        {
                            movement.Velocity = new Point(inPacket.ReadShort(), inPacket.ReadShort());
                            movement.FallStart = inPacket.ReadShort();
                            movement.Stance = inPacket.ReadByte();
                            movement.Duration = inPacket.ReadShort();
                        }
                        break;

                    default:
                        {
                            movement.Stance = inPacket.ReadByte();
                            movement.Duration = inPacket.ReadShort();
                        }
                        break;
                }

                position = movement.Position;
                foothold = movement.Foothold;
                stance = movement.Stance;

                Add(movement);
            }

            byte keypadStates = inPacket.ReadByte();

            for (byte i = 0; i < keypadStates; i++)
            {
                if (i % 2 == 0)
                {
                    inPacket.ReadByte(); // NOTE: Unknown.
                }
            }

            // NOTE: Rectangle for bounds checking.
            inPacket.ReadShort(); // NOTE: Left.
            inPacket.ReadShort(); // NOTE: Top.
            inPacket.ReadShort(); // NOTE: Right.
            inPacket.ReadShort(); // NOTE: Bottom.

            Position = position;
            Stance = stance;
            Foothold = foothold;
        }

        public byte[] ToByteArray()
        {
            using (ByteBuffer oPacket = new ByteBuffer())
            {
                oPacket
                    .WriteShort(Origin.X)
                    .WriteShort(Origin.Y)
                    .WriteByte((byte)Count);

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
