<template>
  <div>
    <div class="relative">
      <input
        class="form-input"
        :id="name"
        :name="name"
        :type="currentType"
        :placeholder="placeholder"
        :value="modelValue"
        @input="$emit('update:modelValue', ($event.target as HTMLInputElement).value)"
        v-bind="$attrs"
      />
      <label for="username">{{ label }}</label>
      <div
        class="suffix"
        v-if="type == 'password'"
        @click="currentType = currentType == 'text' ? 'password' : 'text'"
      >{{ currentType == 'password' ? 'Show' : 'Hide' }}</div>
    </div>
    <div v-if="errorMessage" class="text-red-400 text-sm font-light">{{ errorMessage }}</div>
  </div>
</template>

<script setup lang="ts">
import { PropType, ref } from "vue";
type InputType = 'text' | 'password' | 'email';
const props = defineProps({
  name: { type: String, required: true },
  label: { type: String, required: true },
  placeholder: String,
  type: { type: String as PropType<InputType>, default: 1 },
  errorMessage: String,
  modelValue: String as PropType<String | Number>
});

const currentType = ref<InputType>(props.type);
defineEmits<{
  (event: 'update:modelValue', value: string | number): void
}>()
</script>

<style scoped>
.form-input {
  border: 1px solid gainsboro;
  border-radius: 4px;
  font-size: 12px;
  font-weight: 400;
  background: #fafafa;
  width: 100%;
  height: 40px;
  padding: 14px 0px 2px 8px;
}
.form-input:focus {
  outline: none;
  border-color: #020202c0;
}
.form-input::placeholder {
  transform: scale(0);
}
.form-input:placeholder-shown {
  padding: 9px 0px 7px 8px;
}
.form-input + label {
  font-size: 12px;
  font-weight: 400;
  color: rgb(143, 139, 139);
  display: inline-block;
  transform-origin: top left;
  transform: translate(8px, -20px) scale(0.9);
  transition: all 0.2s;
  position: absolute;
  left: 0;
  bottom: 0;
}
.form-input:placeholder-shown + label {
  transform: translate(8px, -10px);
}

.suffix {
  position: absolute;
  bottom: 0;
  right: 0;
  padding: 10px;
  cursor: pointer;
}
</style>