#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec2 aTexCoord;

out vec2 texCoords;

uniform mat4 model; 
uniform mat4 projection;
uniform mat4 view;

void main()
{
//    vec4 pos =  projection * view * vec4(aPos, 1.0);
    gl_Position = vec4(aPos, 1.0f) * model * view * projection; 
    texCoords = aTexCoord; 
}  