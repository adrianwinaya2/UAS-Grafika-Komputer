#version 330 core
out vec4 outputColor;

in vec2 texCoords;

uniform sampler2D texture0;

void main()
{    
    outputColor = texture(texture0, texCoords);
}