using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static AdventOfCode2018.challenge.MineCartMadness.Cart;
using static AdventOfCode2018.challenge.MineCartMadness.SpecialTrackPiece;

namespace AdventOfCode2018.challenge
{
    class MineCartMadness : Challenge
    {
        public static Point GetFirstCrashLocation()
        {
            Track track = GetTrackInfo();

            Cart crashedCart = null;
            while (crashedCart == null)
            {
                track.Iterate();
                crashedCart = track.crashedCarts.FirstOrDefault();
            }

            return crashedCart.location;
        }

        public static Point GetLastCartLocation()
        {
            Track track = GetTrackInfo();

            while (track.carts.Count - track.crashedCarts.Count > 1)
            {
                track.Iterate();
            }

            return track.carts.Except(track.crashedCarts).First().location;
        }

        private static Track GetTrackInfo()
        {
            Track track = new Track();

            try
            {
                using (StreamReader sr = new StreamReader(GetPath(13)))
                {
                    int y = 0;
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        char previous = ' ';
                        for (int x = 0; x < line.Length; x++)
                        {
                            Point newPoint = new Point(x, y);
                            char letter = line[x];
                            switch (letter)
                            {
                                case ' ':
                                    break;
                                case '-':
                                    break;
                                case '/':
                                    if (previous == '-' || previous == '+')
                                        track.specialTrackPieces.Add(newPoint, new SpecialTrackPiece(newPoint, SpecialTrackPiece.TurnDirection.TOPLEFT));
                                    else
                                        track.specialTrackPieces.Add(newPoint, new SpecialTrackPiece(newPoint, SpecialTrackPiece.TurnDirection.BOTTOMRIGHT));
                                    break;
                                case '\\':
                                    if (previous == '-' || previous == '+')
                                        track.specialTrackPieces.Add(newPoint, new SpecialTrackPiece(newPoint, SpecialTrackPiece.TurnDirection.BOTTOMLEFT));
                                    else
                                        track.specialTrackPieces.Add(newPoint, new SpecialTrackPiece(newPoint, SpecialTrackPiece.TurnDirection.TOPRIGHT));
                                    break;
                                case '+':
                                    track.specialTrackPieces.Add(newPoint, new SpecialTrackPiece(newPoint));
                                    break;
                                case '<':
                                    Cart cartLeft = new Cart(newPoint, CartDirection.LEFT);
                                    cartLeft.OnCartMoved += track.HandleOnCartMoved;
                                    track.carts.Add(cartLeft);
                                    break;
                                case '>':
                                    Cart cartRight = new Cart(newPoint, CartDirection.RIGHT);
                                    cartRight.OnCartMoved += track.HandleOnCartMoved;
                                    track.carts.Add(cartRight);
                                    break;
                                case '^':
                                    Cart cartAbove = new Cart(newPoint, CartDirection.ABOVE);
                                    cartAbove.OnCartMoved += track.HandleOnCartMoved;
                                    track.carts.Add(cartAbove);
                                    break;
                                case 'v':
                                    Cart cartBottom = new Cart(newPoint, CartDirection.BOTTOM);
                                    cartBottom.OnCartMoved += track.HandleOnCartMoved;
                                    track.carts.Add(cartBottom);
                                    break;
                            }

                            previous = letter;
                        }
                        y++;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return track;
        }

        public class Track
        {
            public List<Cart> carts = new List<Cart>();
            public List<Cart> crashedCarts = new List<Cart>();
            public Dictionary<Point, SpecialTrackPiece> specialTrackPieces = new Dictionary<Point, SpecialTrackPiece>();

            public void Iterate()
            {
                foreach (Cart cart in this.carts)
                {
                    if (!crashedCarts.Contains(cart))
                    {
                        cart.Move();
                        if (specialTrackPieces.ContainsKey(cart.location))
                            cart.Turn(specialTrackPieces[cart.location]);
                    }
                }

                this.carts.Sort(Cart.Compare);
            }

            public void HandleOnCartMoved(Object o, EventArgs e)
            {
                Cart cart = o as Cart;
                if (this.carts.Count(c => c.location.Equals(cart.location) && !this.crashedCarts.Contains(c)) > 1)
                {
                    this.crashedCarts.AddRange(this.carts.FindAll(c => c.location.Equals(cart.location) && !this.crashedCarts.Contains(c)));
                }
            }
        }

        public class SpecialTrackPiece
        {
            public enum Types
            {
                INTERSECTION,
                TURN
            }

            public enum TurnDirection
            {
                TOPLEFT = 0,        // -/
                TOPRIGHT = 1,       // \-
                BOTTOMRIGHT = 2,    // /-
                BOTTOMLEFT = 3      // -\
            }

            public Point location;
            public Types type;
            public TurnDirection direction;

            public SpecialTrackPiece(Point location)
            {
                this.location = location;
                this.type = Types.INTERSECTION;
            }

            public SpecialTrackPiece(Point location, TurnDirection direction)
            {
                this.location = location;
                this.type = Types.TURN;
                this.direction = direction;
            }
        }

        public class Point
        {
            public int x;
            public int y;

            public Point(int x, int y)
            {
                this.x = x;
                this.y = y;
            }

            public Point Clone()
            {
                return new Point(this.x, this.y);
            }

            public override int GetHashCode()
            {
                return string.Format("{0},{1}", this.x, this.y).GetHashCode();
            }

            public override bool Equals(object obj)
            {
                Point other = obj as Point;
                return other != null &&  (other.x == this.x && other.y == this.y);
            }
        }

        public class Cart
        {
            public enum CartDirection
            {
                ABOVE = 0,
                LEFT = 1,
                BOTTOM = 2,
                RIGHT = 3
            }

            public enum TurnState
            {
                LEFT = 0,
                STRAIGHT = 1,
                RIGHT = 2
            }

            public Point location;
            public CartDirection direction;
            public TurnState nextTurn;
            
            public EventHandler OnCartMoved;

            public Cart(Point location, CartDirection direction)
            {
                this.location = location;
                this.direction = direction;
                this.nextTurn = TurnState.LEFT;
            }

            public void Move()
            {
                switch (this.direction)
                {
                    case CartDirection.ABOVE:
                        this.location.y--;
                        break;
                    case CartDirection.LEFT:
                        this.location.x--;
                        break;
                    case CartDirection.BOTTOM:
                        this.location.y++;
                        break;
                    case CartDirection.RIGHT:
                        this.location.x++;
                        break;
                }

                this.OnCartMoved.Invoke(this, null);
            }

            public void Turn(SpecialTrackPiece piece)
            {
                if (this.location.x == piece.location.x && this.location.y == piece.location.y)
                {
                    if (piece.type == Types.TURN)
                    {
                        switch (piece.direction)
                        {
                            case TurnDirection.TOPLEFT:
                                if (this.direction == CartDirection.RIGHT)
                                    this.direction = CartDirection.ABOVE;
                                else this.direction = CartDirection.LEFT;
                                break;
                            case TurnDirection.TOPRIGHT:
                                if (this.direction == CartDirection.LEFT)
                                    this.direction = CartDirection.ABOVE;
                                else this.direction = CartDirection.RIGHT;
                                break;
                            case TurnDirection.BOTTOMRIGHT:
                                if (this.direction == CartDirection.LEFT)
                                    this.direction = CartDirection.BOTTOM;
                                else this.direction = CartDirection.RIGHT;
                                break;
                            case TurnDirection.BOTTOMLEFT:
                                if (this.direction == CartDirection.RIGHT)
                                    this.direction = CartDirection.BOTTOM;
                                else this.direction = CartDirection.LEFT;
                                break;
                        }
                    }
                    else if (piece.type == Types.INTERSECTION)
                    {
                        switch (this.nextTurn)
                        {
                            case TurnState.LEFT:
                                switch (this.direction)
                                {
                                    case CartDirection.ABOVE:
                                        this.direction = CartDirection.LEFT;
                                        break;
                                    case CartDirection.LEFT:
                                        this.direction = CartDirection.BOTTOM;
                                        break;
                                    case CartDirection.RIGHT:
                                        this.direction = CartDirection.ABOVE;
                                        break;
                                    case CartDirection.BOTTOM:
                                        this.direction = CartDirection.RIGHT;
                                        break;
                                }
                                this.nextTurn = TurnState.STRAIGHT;
                                break;
                            case TurnState.STRAIGHT:
                                this.nextTurn = TurnState.RIGHT;
                                break;
                            case TurnState.RIGHT:
                                switch (this.direction)
                                {
                                    case CartDirection.ABOVE:
                                        this.direction = CartDirection.RIGHT;
                                        break;
                                    case CartDirection.LEFT:
                                        this.direction = CartDirection.ABOVE;
                                        break;
                                    case CartDirection.RIGHT:
                                        this.direction = CartDirection.BOTTOM;
                                        break;
                                    case CartDirection.BOTTOM:
                                        this.direction = CartDirection.LEFT;
                                        break;
                                }
                                this.nextTurn = TurnState.LEFT;
                                break;
                        }
                    }
                }
            }

            public static int Compare(Cart cart1, Cart cart2)
            {
                if (cart1.location.Equals(cart2.location))
                {
                    return 0;
                }
                else if (cart1.location.y == cart2.location.y && cart2.location.x > cart1.location.x)
                {
                    return 1;
                }
                else if (cart1.location.y > cart2.location.y)
                {
                    return 1;
                }

                return -1;
            }
        }
    }
}
