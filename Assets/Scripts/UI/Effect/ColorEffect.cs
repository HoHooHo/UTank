using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

[ExecuteInEditMode]
[AddComponentMenu("UI/Custom/ColorEffect")]
[RequireComponent(typeof(Image))]
public class ColorEffect : MonoBehaviour {
    private Image _image;
    private Material _material;

    private float[] _matrix;
    private Matrix4x4 _shaderMatrix;
    private Vector4 _offset;
    private bool _isDirty = true;

    public enum BlendType
    {
        Normal,
        None,
        Add,
        Multiply,
        Screen,
        Erase,
        Mask,
        Below,
        Custom
    }

    static float[] IDENTITY = new float[] { 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0 };
    static UnityEngine.Rendering.BlendMode[] Factors = new UnityEngine.Rendering.BlendMode[] {
			//Normal
			UnityEngine.Rendering.BlendMode.SrcAlpha,
			UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha,

			//None
			UnityEngine.Rendering.BlendMode.Zero,
			UnityEngine.Rendering.BlendMode.One,

			//Add
			UnityEngine.Rendering.BlendMode.SrcAlpha,
			UnityEngine.Rendering.BlendMode.One,

			//Multiply
			UnityEngine.Rendering.BlendMode.DstColor,
			UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha,

			//Screen
			UnityEngine.Rendering.BlendMode.SrcAlpha,
			UnityEngine.Rendering.BlendMode.OneMinusSrcColor,

			//Erase
			UnityEngine.Rendering.BlendMode.Zero,
			UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha,

			//Mask
			UnityEngine.Rendering.BlendMode.Zero,
			UnityEngine.Rendering.BlendMode.SrcAlpha,

			//Below
			UnityEngine.Rendering.BlendMode.OneMinusDstAlpha,
			UnityEngine.Rendering.BlendMode.DstAlpha,
		};

    const float LUMA_R = 0.299f;
    const float LUMA_G = 0.587f;
    const float LUMA_B = 0.114f;

    public enum ColorFilterType
    {
        None,
        Invert,
        AdJust,
        Tint,
        Tint2
    }

    [SerializeField]
    private bool m_Gray = false;
    public bool gray
    {
        get { return m_Gray; }
        set { 
           if (value == m_Gray){
            return;
           } 
           m_Gray = value ;
           MarkDirty(); 
        }
    }

    [SerializeField]
    private BlendType m_BlendType = BlendType.Normal;
    public BlendType blendType
    {
        get { return m_BlendType; }
        set
        {
            if (value == m_BlendType)
            {
                return;
            }
            m_BlendType = value;
            MarkDirty();
        }
    }

    [SerializeField]
    public UnityEngine.Rendering.BlendMode m_BlendSrc = UnityEngine.Rendering.BlendMode.SrcAlpha;
    public UnityEngine.Rendering.BlendMode blendSrc
    {
        get { return m_BlendSrc; }
        set
        {
            if (value == m_BlendSrc)
            {
                return;
            }
            m_BlendSrc = value;
            MarkDirty();
        }
    }

     [SerializeField]
    public UnityEngine.Rendering.BlendMode m_BlendDst = UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha;
    public UnityEngine.Rendering.BlendMode blendDst
    {
        get { return m_BlendDst; }
        set
        {
            if (value == m_BlendDst)
            {
                return;
            }
            m_BlendDst = value;
            MarkDirty();
        }
    }

     [SerializeField]
    public ColorFilterType m_ColorFilterType = ColorFilterType.None;
     public ColorFilterType colorFilterType
    {
        get { return m_ColorFilterType; }
        set
        {
            if (value == m_ColorFilterType)
            {
                return;
            }
            m_ColorFilterType = value;
            MarkDirty();
        }
    }

    [Range(-1, 1)]
    [SerializeField]
    private float m_Brightness = 0.0f;
    public float brightness
    {
        get { return m_Brightness; }
        set
        {
            if (value == m_Brightness)
            {
                return;
            }
            m_Brightness = value;
            MarkDirty();
        }
    }

    [Range(-1, 1)]
    [SerializeField]
    public float m_Saturation = 0;
    public float saturation
    {
        get { return m_Saturation; }
        set
        {
            if (value == m_Saturation)
            {
                return;
            }
            m_Saturation = value;
            MarkDirty();
        }
    }

    [Range(-1, 1)]
    [SerializeField]
    public float m_Hue = 0;
    public float hue
    {
        get { return m_Hue; }
        set
        {
            if (value == m_Hue)
            {
                return;
            }
            m_Hue = value;
            MarkDirty();
        }
    }

    [Range(-1, 1)]
    [SerializeField]
    public float m_Contrast = 0;
    public float contrast
    {
        get { return m_Contrast; }
        set
        {
            if (value == m_Contrast)
            {
                return;
            }
            m_Contrast = value;
            MarkDirty();
        }
    }

    [SerializeField]
    private Color m_TintColor = Color.white;
    public Color tintColor
    {
        get { return m_TintColor; }
        set
        {
            if (value == m_TintColor)
            {
                return;
            }
            m_TintColor = value;
            MarkDirty();
        }
    }

    [Range(-1, 1)]
    [SerializeField]
    private float m_TintAmount = 0;
    public float tintAmount
    {
        get { return m_TintAmount; }
        set
        {
            if (value == m_TintAmount)
            {
                return;
            }
            m_TintAmount = value;
            MarkDirty();
        }
    }

    public ColorEffect()
    {
        _matrix = new float[20];
        Array.Copy(IDENTITY, _matrix, _matrix.Length);
    }

    void Reset()
    {
        _InitProperty();
    }

    void OnValidate()
    {
        MarkDirty();
    }

    void OnDidApplyAnimationProperties()
    {
        MarkDirty();
    }

    void MarkDirty()
    {
        _isDirty = true;
    }

    void LateUpdate()
    {
        if (_isDirty) {
            _isDirty = false;
            _UpdateProperty();
        }
    }

    private void _InitProperty()
    {
        _image = this.GetComponent<Image>();

        Shader shader = Shader.Find("UI/ColorEffect");
        Material material = new Material(shader);
        _material = material;
        _image.material = _material;
    }


    private void _UpdateProperty()
    {
        if (_material == null)
        {
            _InitProperty();
        }
        if (m_BlendType != BlendType.Custom) {
            int index = (int)blendType * 2;
            m_BlendSrc = Factors[index];
            m_BlendDst = Factors[index + 1];
        }
        _material.SetFloat("_BlendSrcFactor", (float)m_BlendSrc);
        _material.SetFloat("_BlendDstFactor", (float)m_BlendDst);

        if (m_Gray)
        {
            _material.EnableKeyword("GRAYED");

        }
        else
        {
            _material.DisableKeyword("GRAYED");
        }

        switch (m_ColorFilterType) { 
            case ColorFilterType.None:
                 _material.DisableKeyword("COLOR_FILTER");
                 break;
            case ColorFilterType.Invert:
                 this.ResetMatrix();
                 this.Invert();
                 _material.EnableKeyword("COLOR_FILTER");
                _material.SetMatrix("_ColorMatrix", _shaderMatrix);
                _material.SetVector("_ColorOffset", _offset);
                break;
            case ColorFilterType.AdJust:
                this.ResetMatrix();
                this.AdjustBrightness(m_Brightness);
                this.AdjustContrast(m_Contrast);
                this.AdjustSaturation(m_Saturation);
                this.AdjustHue(m_Hue);

                _material.EnableKeyword("COLOR_FILTER");
                _material.SetMatrix("_ColorMatrix", _shaderMatrix);
                _material.SetVector("_ColorOffset", _offset);
                break;
            case ColorFilterType.Tint:
                this.ResetMatrix();
                this.Tint(m_TintColor, m_TintAmount);

                _material.EnableKeyword("COLOR_FILTER");
                _material.SetMatrix("_ColorMatrix", _shaderMatrix);
                _material.SetVector("_ColorOffset", _offset);
                break;
            case ColorFilterType.Tint2:
                this.ResetMatrix();
                this.AdjustBrightness(m_TintAmount);
                this.Tint(m_TintColor, m_TintAmount);
                //this.Tint2(tintColor, tintAmount);

                _material.EnableKeyword("COLOR_FILTER");
                _material.SetMatrix("_ColorMatrix", _shaderMatrix);
                _material.SetVector("_ColorOffset", _offset);
                break;
        }
    }

    /// <summary>
    /// Changes the saturation. Typical values are in the range (-1, 1).
    /// Values above zero will raise, values below zero will reduce the saturation.
    /// '-1' will produce a grayscale image. 
    /// </summary>
    /// <param name="sat"></param>
    public void AdjustSaturation(float sat)
    {
        if (sat == 0) {
            return;
        }
        sat += 1;

        float invSat = 1 - sat;
        float invLumR = invSat * LUMA_R;
        float invLumG = invSat * LUMA_G;
        float invLumB = invSat * LUMA_B;

        ConcatValues((invLumR + sat), invLumG, invLumB, 0, 0,
                      invLumR, (invLumG + sat), invLumB, 0, 0,
                      invLumR, invLumG, (invLumB + sat), 0, 0,
                      0, 0, 0, 1, 0);
    }

    /// <summary>
    /// Changes the contrast. Typical values are in the range (-1, 1).
    /// Values above zero will raise, values below zero will reduce the contrast.
    /// </summary>
    /// <param name="value"></param>
    public void AdjustContrast(float value)
    {
        if (value == 0)
        {
            return;
        }
        float s = value + 1;
        float o = 128f / 255 * (1 - s);

        ConcatValues(s, 0, 0, 0, o,
                     0, s, 0, 0, o,
                     0, 0, s, 0, o,
                     0, 0, 0, 1, 0);
    }

    /// <summary>
    /// Changes the brightness. Typical values are in the range (-1, 1).
    /// Values above zero will make the image brighter, values below zero will make it darker.
    /// </summary>
    /// <param name="value"></param>
    public void AdjustBrightness(float value)
    {
        if (value == 0)
        {
            return;
        }
        ConcatValues(1, 0, 0, 0, value,
                     0, 1, 0, 0, value,
                     0, 0, 1, 0, value,
                     0, 0, 0, 1, 0);
    }

    /// <summary>
    ///Changes the hue of the image. Typical values are in the range (-1, 1).
    /// </summary>
    /// <param name="value"></param>
    public void AdjustHue(float value)
    {
        if (value == 0)
        {
            return;
        }
        value *= Mathf.PI;

        float cos = Mathf.Cos(value);
        float sin = Mathf.Sin(value);

        ConcatValues(
            ((LUMA_R + (cos * (1 - LUMA_R))) + (sin * -(LUMA_R))), ((LUMA_G + (cos * -(LUMA_G))) + (sin * -(LUMA_G))), ((LUMA_B + (cos * -(LUMA_B))) + (sin * (1 - LUMA_B))), 0, 0,
            ((LUMA_R + (cos * -(LUMA_R))) + (sin * 0.143f)), ((LUMA_G + (cos * (1 - LUMA_G))) + (sin * 0.14f)), ((LUMA_B + (cos * -(LUMA_B))) + (sin * -0.283f)), 0, 0,
            ((LUMA_R + (cos * -(LUMA_R))) + (sin * -((1 - LUMA_R)))), ((LUMA_G + (cos * -(LUMA_G))) + (sin * LUMA_G)), ((LUMA_B + (cos * (1 - LUMA_B))) + (sin * LUMA_B)), 0, 0,
            0, 0, 0, 1, 0);
    }

    /// <summary>
    /// Tints the image in a certain color, analog to what can be done in Adobe Animate.
    /// </summary>
    /// <param name="color">the RGB color with which the image should be tinted.</param>
    /// <param name="amount">the intensity with which tinting should be applied. Range (0, 1).</param>
    public void Tint(Color color, float amount = 1.0f)
    {
        float q = 1 - amount;

        float rA = amount * color.r;
        float gA = amount * color.g;
        float bA = amount * color.b;

        ConcatValues(
            q + rA * LUMA_R, rA * LUMA_G, rA * LUMA_B, 0, 0,
            gA * LUMA_R, q + gA * LUMA_G, gA * LUMA_B, 0, 0,
            bA * LUMA_R, bA * LUMA_G, q + bA * LUMA_B, 0, 0,
            0, 0, 0, 1, 0);
    }

    public void Tint2(Color color, float amount = 1.0f)
    {
        float q = 1 - amount;

        float rA = amount * color.r;
        float gA = amount * color.g;
        float bA = amount * color.b;

        ConcatValues(
           q + rA, rA, rA, 0, 0,
           gA, q + gA, gA, 0, 0,
           bA, bA, q + bA, 0, 0,
           0, 0, 0, 1, 0);
    }

    /// <summary>
    /// Changes the filter matrix back to the identity matrix
    /// </summary>
    public void ResetMatrix()
    {
        Array.Copy(IDENTITY, _matrix, _matrix.Length);

        UpdateMatrix();
    }

    static float[] tmp = new float[20];

    /// <summary>
    /// 
    /// </summary>
    /// <param name="values"></param>
    public void ConcatValues(params float[] values)
    {
        int i = 0;

        for (int y = 0; y < 4; ++y)
        {
            for (int x = 0; x < 5; ++x)
            {
                tmp[i + x] = values[i] * _matrix[x] +
                        values[i + 1] * _matrix[x + 5] +
                        values[i + 2] * _matrix[x + 10] +
                        values[i + 3] * _matrix[x + 15] +
                        (x == 4 ? values[i + 4] : 0);
            }
            i += 5;
        }
        Array.Copy(tmp, _matrix, tmp.Length);

        UpdateMatrix();
    }

    public void Invert()
    {
        ConcatValues(-1, 0, 0, 0, 1,
                      0, -1, 0, 0, 1,
                      0, 0, -1, 0, 1,
                      0, 0, 0, 1, 0);
    }

    void UpdateMatrix()
    {
        _shaderMatrix.SetRow(0, new Vector4(_matrix[0], _matrix[1], _matrix[2], _matrix[3]));
        _shaderMatrix.SetRow(1, new Vector4(_matrix[5], _matrix[6], _matrix[7], _matrix[8]));
        _shaderMatrix.SetRow(2, new Vector4(_matrix[10], _matrix[11], _matrix[12], _matrix[13]));
        _shaderMatrix.SetRow(3, new Vector4(_matrix[15], _matrix[16], _matrix[17], _matrix[18]));
        _offset = new Vector4(_matrix[4], _matrix[9], _matrix[14], _matrix[19]);
    }
}
