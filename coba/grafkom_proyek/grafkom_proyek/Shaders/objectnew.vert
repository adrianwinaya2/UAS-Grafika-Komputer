﻿#version 330 core

layout(location = 0) in vec3 aPosition;

//layout(location = 1) in vec3 aColor;
layout(location = 1) in vec3 aNormal;

//Menyediakan variabel yang bisa dikirim ke next step (.frag)
out vec4 FragPos;
out vec3 Normal;
//out vec3 FragPos;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main(void)
{
	gl_Position = vec4(aPosition, 1.0) * model * view * projection;
//	Normal = aNormal;
//	FragPos = vec3(model * vec4(aPosition, 1.0));

	
	FragPos = vec4(aPosition, 1.0) * model;
	Normal = aNormal * mat3(transpose(inverse(model)));
}