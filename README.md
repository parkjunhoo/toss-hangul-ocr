# -
<img src="https://img1.daumcdn.net/thumb/R1280x0/?scode=mtistory2&fname=https%3A%2F%2Fblog.kakaocdn.net%2Fdn%2FcD5Swt%2FbtsJYdu6wEY%2Fmw5WRbgf5cKeetizSS33Mk%2Fimg.jpg" height="600"/>
<br/>

## 토스에서 한글날이라 틀린 글자 찾기 이벤트를 했따.. 도저히 1~3단계 까진 풀만한데

<br />

##  다음부턴 도저히 사람이 풀 수 없는 난이도... 5단계까지 풀면 만원준다는데
<br/>
## 사람 약올리는거같고;;; 뭔가 근데 단순해서 프로그램으로 될거같아서 이건 못참지
<img src="https://blog.kakaocdn.net/dn/btUrD9/btsJX8ACWDy/3z5lgEHci9OvYBSSmyDap1/img.webp" />
<br>
### 1.문제 나오는 부분만 짜르기 <br>
### 2.흰색만 남겨서 이미지전처리후 테두리 검출해서 영역 검출 <br>
### 3.각 영역의 중앙값 찾기 ( 클릭해야되는 좌표를 알아야대니깐? 실제 핸드폰이랑 연동은안햇다,,) <br>
### 4.ocr로 검출하던 어떤 규칙을 찾아서 찾던 하기, <br>

# 이걸 써서 만원 받진 않앗음,,

<br/><br/><br/><br/><br/><br/><br/>

## 써본방법

<br>
### 1.이미지 이진화해서 범위안에서 검정색 픽셀 찾아서 글자의 픽셀 수 세어서 다른글자 찾기
<br>
### 2.ocr로도 해봣다, 테서렉트 ← 맨첨에 했을땐 잘안됬는데 진짜 딱 깔끔하게 글자만 남기고 블러처리하니 인식잘됨, 
