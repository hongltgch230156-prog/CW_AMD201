<!-- src/components/PopupCenter.vue -->
<template>
  <transition name="fade-scale">
    <div v-if="visible" class="popup-overlay">
      <div class="popup-box" :class="typeClass">
        <h3 class="popup-title">{{ titleText }}</h3>
        <p class="popup-message">{{ message }}</p>

        <div class="popup-actions">
          <button @click="close" class="btn-ok">OK</button>
        </div>
      </div>
    </div>
  </transition>
</template>

<script setup>
import { ref, computed } from 'vue';

const visible = ref(false);
const message = ref('');
const type = ref('success'); // 'success' | 'error' | 'info'
const titleText = ref('Thông báo');

const typeClass = computed(() => {
  return type.value === 'success'
    ? 'popup-success'
    : type.value === 'error'
    ? 'popup-error'
    : 'popup-info';
});

// Hàm để component cha gọi: popupRef.value.show(...)
function show(msg, t = 'success', title = 'Thông báo') {
  message.value = msg || '';
  type.value = t || 'success';
  titleText.value = title || 'Thông báo';
  visible.value = true;
}

function close() {
  visible.value = false;
}

defineExpose({ show, close });
</script>

<style scoped>
.popup-overlay {
  position: fixed;
  inset: 0;
  background: rgba(0,0,0,0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 9999;
}

.popup-box {
  width: 90%;
  max-width: 420px;
  background: #fff;
  border-radius: 12px;
  padding: 22px;
  box-shadow: 0 10px 30px rgba(0,0,0,0.25);
  text-align: center;
}

.popup-title {
  font-size: 20px;
  margin-bottom: 12px;
  color: #1f2937;
}

.popup-message {
  font-size: 15px;
  color: #374151;
  margin-bottom: 18px;
  white-space: pre-wrap;
}

/* Buttons */
.popup-actions {
  display: flex;
  justify-content: center;
}

.btn-ok {
  background: #42b983;
  color: white;
  padding: 10px 20px;
  border: none;
  border-radius: 8px;
  cursor: pointer;
  font-weight: 600;
}
.btn-ok:hover { filter: brightness(0.95); }

/* Type accent */
.popup-success { border-top: 6px solid #22c55e; }
.popup-error   { border-top: 6px solid #ef4444; }
.popup-info    { border-top: 6px solid #3b82f6; }

/* animation */
.fade-scale-enter-active,
.fade-scale-leave-active {
  transition: all 0.22s ease;
}
.fade-scale-enter-from {
  opacity: 0;
  transform: scale(0.96);
}
.fade-scale-leave-to {
  opacity: 0;
  transform: scale(0.96);
}
</style>
