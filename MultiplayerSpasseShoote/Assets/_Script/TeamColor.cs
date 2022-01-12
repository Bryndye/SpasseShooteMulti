using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Team Color")]
public class TeamColor : ScriptableObject
{
    public Color ColorA, ColorMe, ColorE;

    public Color SetColorTeam(Teams _Team, bool _isLocalPlayer = false)
    {
        Color _c;
        if (_isLocalPlayer)
        {
            return ColorMe;
            
        }
        switch (_Team)
        {
            case Teams.NoTeam:
                _c = ColorE;
                break;

            case Teams.Blue:
                _c = ColorA;
                break;

            case Teams.Red:
                _c = ColorE;
                break;

            default:
                _c = Color.white;
                break;
        }
        return _c;
    }
}
