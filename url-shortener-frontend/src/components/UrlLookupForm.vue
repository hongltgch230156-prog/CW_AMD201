<script setup>
import { ref } from 'vue';
import { getUrlDetails } from '../services/api.js'; 

// Dùng event 'url-found' để gửi dữ liệu lên App.vue
const emit = defineEmits(['url-found']); 

const shortUrl = ref('');
const isLoading = ref(false);
const errorMessage = ref('');
const successMessage = ref('');

const lookupOriginalUrl = async () => {
  errorMessage.value = '';
  successMessage.value = '';
  
  if (!shortUrl.value) {
    errorMessage.value = 'Vui lòng nhập URL ngắn hoặc mã code.';
    return;
  }
  
  isLoading.value = true;
  
  // Logic trích xuất Short Code
  let shortCode = shortUrl.value.trim();
  // Thử trích xuất mã code nếu người dùng dán URL đầy đủ
  const match = shortUrl.value.match(/\/api\/url\/([a-zA-Z0-9]+)$/i); 
  if (match && match[1]) {
    shortCode = match[1];
  }

  if (!shortCode) {
    errorMessage.value = 'Mã code không hợp lệ.';
    isLoading.value = false;
    return;
  }
  
  try {
    // Gọi hàm API đặc biệt để lấy chi tiết URL gốc
    const result = await getUrlDetails(shortCode);

    // Phát ra event với format chuẩn { shortUrl, originalUrl }
    emit('url-found', {
      shortUrl: result.Url,         // URL ngắn đầy đủ
      originalUrl: result.originalUrl // URL gốc
    });
    
    successMessage.value = `Đã tìm thấy URL gốc: ${result.originalUrl}`;
    shortUrl.value = ''; // Xóa input

  } catch (error) {
    console.error('Lỗi tìm kiếm URL:', error);
    const apiError = error.message || 'Lỗi không xác định.';
    errorMessage.value = apiError;
  } finally {
    isLoading.value = false;
  }
};
</script>

<template>
  <form @submit.prevent="lookupOriginalUrl" class="lookup-wrapper">
    <input
      v-model="shortUrl"
      type="text"
      placeholder="Nhập URL ngắn..."
      class="lookup-input"
      required
    />

    <button
      type="submit"
      :disabled="isLoading"
      class="lookup-btn"
    >
      <span v-if="isLoading">Đang tìm kiếm...</span>
      <span v-else>Mở rộng</span>
    </button>
    
    <p v-if="errorMessage" class="msg-error">{{ errorMessage }}</p>
    <p v-if="successMessage" class="msg-error">{{ successMessage }}</p>
  </form>
</template>

<style scoped>
.lookup-wrapper {
  width: 100%;
  max-width: 600px;
  margin: auto;
  background: white;
  padding: 24px;
  border-radius: 14px;
  box-shadow: 0 4px 14px rgba(0,0,0,0.08);
}

/** INPUT **/
.lookup-input {
  width: 100%;
  padding: 14px;
  border-radius: 10px;
  border: 1px solid #d1d5db;
  font-size: 16px;
  outline: none;
  box-sizing: border-box;
  transition: 0.2s ease;
}

.lookup-input:focus {
  border-color: #42b983;
  box-shadow: 0 0 0 3px rgba(66,185,131,0.25);
}

/** BUTTON **/
.lookup-btn {
  width: 100%;
  margin-top: 12px;
  padding: 14px;
  background: #42b983;
  color: white;
  font-size: 16px;
  font-weight: 600;
  border-radius: 10px;
  transition: 0.2s;
}

.lookup-btn:hover {
  background: #369d6d;
}

.lookup-btn:disabled {
  background: #8bd8b5;
  cursor: not-allowed;
}

/** MESSAGES **/
.msg-error {
  margin-top: 10px;
  color: #e63946;
  text-align: center;
  font-size: 14px;
}

.msg-success {
  margin-top: 10px;
  color: #2a9d8f;
  text-align: center;
  font-size: 14px;
}
</style>
